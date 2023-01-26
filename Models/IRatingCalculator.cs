namespace RatingCalculator.Models;

/// <summary>
/// A calculator for evaluating entities (participants) in games and tournaments
/// based solely on game outcomes and who played who.
/// </summary>
/// <typeparam name="TEntity">The type that the entities are identified by</typeparam>
public interface IRatingCalculator<TEntity>
{
    /// <summary>
    /// Calculate the ratings for all entities given the list of games.
    /// </summary>
    /// <param name="games">List of games to consider for calculating the ratings</param>
    /// <returns>An object facilitating the retrieval of the rating for a certain entity or group</returns>
    IRatingResult<TEntity> CalculateRatings(IEnumerable<Game<TEntity>> games);
}