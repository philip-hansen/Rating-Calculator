﻿namespace RatingCalculator.Beta;

internal class ExpectedResultCalculator
{
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
}