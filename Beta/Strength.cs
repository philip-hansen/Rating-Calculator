namespace RatingCalculator.Beta;

internal readonly record struct Strength
{
    private readonly int _value;

    public int Value => _value;

    public double NormalizedValue => (double)_value / BetaRatingProbabilities.SIZE;

    public Strength(int value)
    {
        if (value < 1 || value > BetaRatingProbabilities.SIZE - 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        _value = value;
    }
}