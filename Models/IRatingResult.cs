namespace RatingCalculator.Models;

/// <summary>
/// An object facilitating the retrieval of a specific entity or group
/// </summary>
/// <typeparam name="TEntity">Type the entity is identified by</typeparam>
public interface IRatingResult<TEntity>
{
    /// <summary>
    /// Get the rating of a specific entity
    /// </summary>
    /// <param name="player">Entity to get the rating for</param>
    /// <returns>The given entity's rating</returns>
    IRating Get(TEntity player);

    /// <summary>
    /// Get the combined rating of a specific group of entities.
    /// This is equivalent to choosing a member of the group at random (equal weight) and using it for calculations.
    /// This is useful when a comparison between e.g. conferences or divisions is requested.
    /// </summary>
    /// <param name="entities">Group of entities to combine into a single rating</param>
    /// <returns>The combined entities' rating</returns>
    IRating GetGroup(IEnumerable<TEntity> entities);
}