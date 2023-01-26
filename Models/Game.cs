namespace RatingCalculator.Models;

/// <summary>
/// Represents a single game for the purposes of ratings calculations
/// </summary>
/// <typeparam name="TEntity">Type the game's participants are identified by</typeparam>
public class Game<TEntity>
{
    /// <summary>
    /// The first player to participate
    /// </summary>
    public TEntity HomeEntity { get; init; }

    /// <summary>
    /// The first player's opponent
    /// </summary>
    public TEntity AwayEntity { get; init; }

    /// <summary>
    /// The outcome of the game
    /// </summary>
    public GameResult Result { get; init; }

    public Game(TEntity home, TEntity away, GameResult result)
    {
        HomeEntity = home;
        AwayEntity = away;
        Result = result;
    }
}