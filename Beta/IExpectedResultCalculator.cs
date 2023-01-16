namespace RatingCalculator.Beta;

internal interface IExpectedResultCalculator
{
    // TODO: Split?
    double CalculateStrengthExpectedResult(Strength me, Strength you);

    //double CalculateRatingExpectedResult(BetaRating me, BetaRating you);
}