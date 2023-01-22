namespace RatingCalculator.Beta;

internal readonly record struct Strength
{
    private readonly int _value;

    private readonly int _size;

    public int Value => _value;

    public double NormalizedValue => (double)_value / _size;

    public Strength(int value, int size)
    {
        _value = value;
        _size = size;
    }
}