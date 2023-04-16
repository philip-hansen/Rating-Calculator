using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal record TeamGame(StrengthProbabilityDistribution Opponent, MyGameResult Result);

internal enum MyGameResult
{
    Win,
    Loss,
    Draw,
}

internal static class MyGameUtil
{
    public static MyGameResult ToMyGameHomeResult(this GameResult result)
    {
        return result switch
        {
            GameResult.HomeWin => MyGameResult.Win,
            GameResult.AwayWin => MyGameResult.Loss,
            GameResult.Draw => MyGameResult.Draw,
            _ => throw new Exception("Unknown value for result")
        };
    }

    public static MyGameResult ToMyGameAwayResult(this GameResult result)
    {
        return result switch
        {
            GameResult.HomeWin => MyGameResult.Loss,
            GameResult.AwayWin => MyGameResult.Win,
            GameResult.Draw => MyGameResult.Draw,
            _ => throw new Exception("Unknown value for result")
        };
    }
}