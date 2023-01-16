namespace RatingCalculator.Beta;

internal class BetaRatingProbabilities
{
    // TODO: Not make global const
    public const int SIZE = 1000;

    private readonly double[] _probabilities;

    public BetaRatingProbabilities()
    {
        _probabilities = new double[SIZE - 2];
    }

    public double Get(Strength strength)
    {
        return _probabilities[strength.Value - 1];
    }

    public void Set(Strength strength, double value)
    {
        _probabilities[strength.Value] = value;
    }

    public void ForEach(Action<Strength> f)
    {
        for (int i = 0; i < _probabilities.Length; i++)
        {
            f(new(i + 1));
        }
    }

    public void Normalize()
    {
        var sum = _probabilities.Sum();

        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] /= sum;
        }
    }
}