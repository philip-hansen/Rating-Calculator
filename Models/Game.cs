namespace RatingCalculator.Models;

public class Game<TEntity>
{
    public TEntity HomeEntity { get; init; }

    public TEntity AwayEntity { get; init; }

    public GameResult Result { get; init; }

    public Game(TEntity home, TEntity away, GameResult result)
    {
        HomeEntity = home;
        AwayEntity = away;
        Result = result;
    }
}