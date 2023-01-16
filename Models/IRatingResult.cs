namespace RatingCalculator.Models;

public interface IRatingResult<TPlayer, TRating>
{
    TRating Get(TPlayer player);
}