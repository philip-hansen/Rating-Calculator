using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class StrengthProbabilityDistribution
{
    // TODO: Not make global const
    public const int SIZE = 1001;

    private readonly double[] _probabilities;

    public StrengthProbabilityDistribution(
        IEnumerable<MyGame> games,
        IExpectedResultCalculator calculator)
    {
        _probabilities = new double[SIZE - 1];

        ForEach(strength =>
        {
            _probabilities[strength.Value - 1] = ProbabilityOfResults(strength, calculator, games);
        });

        Normalize();
    }

    public StrengthProbabilityDistribution()
    {
        // Default prior distribution
        // Uniform across [0, 1]
        _probabilities = new double[SIZE - 1];
        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] = 1.0 / _probabilities.Length;
        }
    }

    // Technically a mass, not a density...
    public double Density(Strength strength)
    {
        if (strength.Value < 1 || strength.Value >= SIZE)
        {
            return 0.0;
        }
        return _probabilities[strength.Value - 1];
    }

    public Strength Cumulative(double probability)
    {
        double cumulativeProbability = 0.0;

        for (int i = 1; i < SIZE; i++)
        {
            cumulativeProbability += Density(new Strength(i));

            if (cumulativeProbability > probability)
            {
                return new Strength(i);
            }
        }

        return new Strength(SIZE);
    }

    public void ForEach(Action<Strength> f)
    {
        for (int i = 1; i < SIZE; i++)
        {
            f(new Strength(i));
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