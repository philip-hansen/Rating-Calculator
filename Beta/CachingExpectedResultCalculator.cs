using RatingCalculator.Beta;

internal class CachingExpectedResultCalculator : ExpectedResultCalculator
{
    private readonly double[,] _cachedProbabilities;

    public CachingExpectedResultCalculator(int size) : base(size)
    {
        _cachedProbabilities = new double[size, size];

        for (int x = 1; x < size; x++)
        {
            for (int y = 1; y < size; y++)
            {
                _cachedProbabilities[x, y] = base.CalculateExpectedResult(new Strength(x, size), new Strength(y, size));
            }
        }
    }

    public override double CalculateExpectedResult(Strength me, Strength you) => _cachedProbabilities[me.Value, you.Value];
}