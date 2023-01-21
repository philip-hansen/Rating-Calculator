namespace RatingCalculator.Beta;

internal class StrengthProbabilityDistribution
{
    private readonly int _size;

    private readonly double[] _probabilities;

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
    }

    public StrengthProbabilityDistribution(int size)
    {
        _size = size;

        // Default prior distribution
        // Uniform across [0, 1]
        _probabilities = new double[_size - 1];
        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] = 1.0 / _probabilities.Length;
        }
    }

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