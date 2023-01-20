namespace RatingCalculator.Beta;

internal readonly record struct Strength
{
    private readonly int _value;

    private readonly int _size;

    public int Value => _value;

    public double NormalizedValue => (double)_value / _size;

    public Strength(int value, int size)
    {
        if (value < 0 || value > size)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        _value = value;
        _size = size;
    }
}