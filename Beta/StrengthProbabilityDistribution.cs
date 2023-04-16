namespace RatingCalculator.Beta;

internal class StrengthProbabilityDistribution
{
    private readonly int _size;

    private readonly double[] _probabilities;

    private readonly double _expectedResultAgainstAverage;

    public StrengthProbabilityDistribution(
        IEnumerable<MyGame> games,
        ExpectedResultCalculator calculator,
        int size)
    {
        _size = size;

        _probabilities = new double[_size - 1];

        ForEach(strength =>
        {
            _probabilities[strength.Value - 1] = ProbabilityOfResults(strength, calculator, games);
        });

        Normalize();

        _expectedResultAgainstAverage = calculator.CalculateAverageExpectedResult(this);
    }

    public StrengthProbabilityDistribution(int size)
    {
        _size = size;

        // Default prior distribution
        // Uniform across (0, 1)
        _probabilities = new double[_size - 1];
        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] = 1.0 / _probabilities.Length;
        }
    }

    public static StrengthProbabilityDistribution FromGroup(IEnumerable<StrengthProbabilityDistribution> members)
    {
        var memberTotal = members.Count();
        
        // They all should have the same size (otherwise it's undefined anyway),
        // so using the first should be safe
        var size = members.First()._size;

        StrengthProbabilityDistribution combinedDistribution = new(size);

        combinedDistribution.ForEach(s =>
        {
            double probability = 0.0;
            foreach (var member in members)
            {
                probability += member.Density(s) / memberTotal;
            }

            combinedDistribution._probabilities[s.Value - 1] = probability;
        });

        return combinedDistribution;
    }

    public double ExpectedResultAgainstAverage => _expectedResultAgainstAverage;

    // Technically a mass, not a density...
    public double Density(Strength strength)
    {
        if (strength.Value < 1 || strength.Value >= _size)
        {
            return 0.0;
        }
        return _probabilities[strength.Value - 1];
    }

    public double Cumulative(Strength strength)
    {
        double cumulativeProbability = 0.0;

        for (int i = 1; i <= strength.Value; i++)
        {
            cumulativeProbability += Density(new Strength(i, _size));
        }

        return cumulativeProbability;
    }

    public void ForEach(Action<Strength> f)
    {
        for (int i = 1; i < _size; i++)
        {
            f(new Strength(i, _size));
        }
    }

    public double Mean()
    {
        double total = 0.0;
        ForEach(s =>
        {
            total += s.NormalizedValue * Density(s);
        });

        return total;
    }

    public double Mode()
    {
        // Value doesn't matter; guaranteed to be overwritten
        Strength maxStrength = new(1, 1);
        double maxProbability = 0;

        ForEach(s =>
        {
            double density = Density(s);
            if (density > maxProbability)
            {
                maxStrength = s;
                maxProbability = density;
            }
        });

        return maxStrength.NormalizedValue;
    }

    public double Variance()
    {
        double mean = Mean();
        double variance = 0.0;

        ForEach(s =>
        {
            double diffSquared = (s.NormalizedValue - mean) * (s.NormalizedValue - mean);

            variance += diffSquared * Density(s);
        });

        return variance;
    }

    public double Median()
    {
        double cumulative = 0.0;
        for (int i = 1; i < _size; i++)
        {
            var strength = new Strength(i, _size);
            cumulative += Density(strength);
            if (cumulative > 0.5)
            {
                return strength.NormalizedValue;
            }
        }

        // Should not happen
        return 0.0;
    }

    private static double ProbabilityOfResults(
        Strength strength, 
        ExpectedResultCalculator calculator, 
        IEnumerable<MyGame> games)
    {
        double totalProbability = 1.0;

        foreach (MyGame game in games)
        {
            double expectedResults = calculator.CalculateExpectedResult(strength, game.Opponent);
            double probabilityOfResult = game.Result switch
            {
                MyGameResult.Win => expectedResults,
                MyGameResult.Loss => 1 - expectedResults,
                MyGameResult.Draw => Math.Sqrt(expectedResults * (1 - expectedResults)),
                _ => throw new Exception("Invalid enum value for game result")
            };
            totalProbability *= probabilityOfResult;
        }

        return totalProbability;
    }

    private void Normalize()
    {
        var sum = _probabilities.Sum();

        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] /= sum;
        }
    }
}