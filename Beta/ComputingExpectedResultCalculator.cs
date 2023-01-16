namespace RatingCalculator.Beta;

internal class ComputingExpectedResultCalculator : IExpectedResultCalculator
{
    public double CalculateStrengthExpectedResult(Strength me, Strength you)
    {
        double myStrength = me.NormalizedValue;
        double yourStrength = you.NormalizedValue;

        double iWin = myStrength * (1 - yourStrength);
        double youWin = yourStrength * (1 - myStrength);

        return iWin / (iWin + youWin);
    }
}