namespace RatingCalculator.Models;

public class Game<TPlayer> where TPlayer : notnull
{
    public TPlayer HomePlayer { get; init; }

    public TPlayer AwayPlayer { get; init; }

    public GameResult Result { get; init; }

    public Game(TPlayer home, TPlayer away, GameResult result)
    {
        HomePlayer = home;
        AwayPlayer = away;
        Result = result;
    }
}

public enum GameResult
{
    HomeWin,
    AwayWin,
    Draw,
}