namespace RatingCalculator.Models;

public interface IRatingResult<TEntity>
{
    IRating Get(TEntity player);
}