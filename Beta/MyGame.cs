namespace RatingCalculator.Beta;

internal record MyGame(StrengthProbabilityDistribution Opponent, MyGameResult Result);

internal enum MyGameResult
{
    Win,
    Loss,
    Draw,
}