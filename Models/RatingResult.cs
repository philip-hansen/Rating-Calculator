namespace RatingCalculator.Models;

internal class RatingResult<TPlayer, TRating> : IRatingResult<TPlayer, TRating> where TPlayer : notnull
{
    private readonly IDictionary<TPlayer, TRating> _ratings;
    private readonly TRating _defaultRating;

    public RatingResult(TRating defaultRating)
    {
        _ratings = new Dictionary<TPlayer, TRating>();
        _defaultRating = defaultRating;
    }

    public TRating Get(TPlayer player)
    {
        if (!_ratings.ContainsKey(player))
        {
            return _defaultRating;
        }

        return _ratings[player];
    }

    public void Set(TPlayer player, TRating rating)
    {
        _ratings[player] = rating;
    }
}