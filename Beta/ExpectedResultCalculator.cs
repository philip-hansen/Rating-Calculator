namespace RatingCalculator.Beta;

internal class ExpectedResultCalculator
{
    private readonly int _size;

    public ExpectedResultCalculator(int size)
    {
        _size = size;
    }

    public virtual double CalculateExpectedResult(Strength me, Strength you)
    {
        double myStrength = me.NormalizedValue;
        double yourStrength = you.NormalizedValue;

        double iWin = myStrength * (1 - yourStrength);
        double youWin = yourStrength * (1 - myStrength);

        return iWin / (iWin + youWin);
    }

    public double CalculateExpectedResult(Strength myStrength, StrengthProbabilityDistribution you)
    {
        double expectedResult = 0.0;

        you.ForEach(yourStrength =>
        {
            double yourStrengthProbability = you.Density(yourStrength);
            expectedResult += yourStrengthProbability * CalculateExpectedResult(myStrength, yourStrength);
        });

        return expectedResult;
    }

    public double CalculateAverageExpectedResult(StrengthProbabilityDistribution you) => 
        CalculateExpectedResult(new Strength(_size / 2, _size), you);
}