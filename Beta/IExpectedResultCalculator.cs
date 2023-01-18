namespace RatingCalculator.Beta;

internal interface IExpectedResultCalculator
{
    double CalculateExpectedResult(Strength me, Strength you);

    double CalculateExpectedResult(Strength me, StrengthProbabilityDistribution you);

    double CalculateExpectedResult(StrengthProbabilityDistribution me, StrengthProbabilityDistribution you);
}