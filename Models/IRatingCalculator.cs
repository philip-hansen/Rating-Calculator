namespace RatingCalculator.Models;

public interface IRatingCalculator<TEntity>
{
    IRatingResult<TEntity> CalculateRatings(IEnumerable<Game<TEntity>> games);
}