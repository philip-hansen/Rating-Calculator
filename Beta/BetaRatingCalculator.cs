using RatingCalculator.Models;

namespace RatingCalculator.Beta;

class BetaRatingCalculator<TEntity> : IRatingCalculator<TEntity> where TEntity : IEquatable<TEntity>
{
    public IRatingResult<TEntity> CalculateRatings(IEnumerable<Game<TEntity>> games)
    {
        return new BetaRatingResult<TEntity>(new Dictionary<TEntity, StrengthProbabilityDistribution>());
    }
}