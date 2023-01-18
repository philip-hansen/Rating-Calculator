namespace RatingCalculator.Beta;

internal class StrengthProbabilityDistribution
{
    // TODO: Not make global const
    public const int SIZE = 1000;

    private readonly double[] _probabilities;

    public StrengthProbabilityDistribution()
    {
        _probabilities = new double[SIZE - 2];
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

    private void Normalize()
    {
        var sum = _probabilities.Sum();

        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] /= sum;
        }
    }
}