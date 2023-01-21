using RatingCalculator.Beta;

internal class CachingExpectedResultCalculator : ExpectedResultCalculator
{
    private readonly double?[,] _cachedProbabilities;

    public CachingExpectedResultCalculator(int size)
    {
        _cachedProbabilities = new double?[size, size];
    }

    public override double CalculateExpectedResult(Strength me, Strength you)
    {
        if (_cachedProbabilities[me.Value - 1, you.Value - 1] is null)
        {
            _cachedProbabilities[me.Value - 1, you.Value - 1] = base.CalculateExpectedResult(me, you);
        }

        return _cachedProbabilities[me.Value - 1, you.Value - 1]!.Value;
    }
}