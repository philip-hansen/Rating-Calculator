using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class StrengthProbabilityDistribution
{
    // TODO: Not make global const
    public const int SIZE = 1000;

    private readonly double[] _probabilities;

    public StrengthProbabilityDistribution(
        IEnumerable<MyGame> games,
        IExpectedResultCalculator calculator)
    {
        _probabilities = new double[SIZE - 2];

        ForEach(strength =>
        {
            _probabilities[strength.Value] = ProbabilityOfResults(strength, calculator, games);
        });

        Normalize();
    }

    public StrengthProbabilityDistribution()
    {
        // Default prior distribution
        // Uniform across [0, 1]
        _probabilities = new double[SIZE - 2];
        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] = 1.0 / _probabilities.Length;
        }
    }

    // Technically a mass, not a density...
    public double Density(Strength strength)
    {
        return _probabilities[strength.Value - 1];
    }

    public Strength Cumulative(double probability)
    {
        double cumulativeProbability = 0.0;

        for (int i = 1; i < _probabilities.Length; i++)
        {
            cumulativeProbability += Density(new Strength(i));

            if (cumulativeProbability > probability)
            {
                return new Strength(i - 1);
            }
        }

        return new Strength(SIZE);
    }

    public void ForEach(Action<Strength> f)
    {
        for (int i = 0; i < _probabilities.Length; i++)
        {
            f(new Strength(i + 1));
        }
    }

    private static double ProbabilityOfResults(
        Strength strength, 
        IExpectedResultCalculator calculator, 
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