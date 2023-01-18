namespace RatingCalculator.Beta;

internal readonly record struct Strength
{
    private readonly int _value;

    public int Value => _value;

    public double NormalizedValue => (double)_value / StrengthProbabilityDistribution.SIZE;

    public Strength(int value)
    {
        if (value < 0 || value > StrengthProbabilityDistribution.SIZE)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        _value = value;
    }
}