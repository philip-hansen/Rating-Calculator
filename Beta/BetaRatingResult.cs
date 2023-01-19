using RatingCalculator.Models;
using System.Collections.ObjectModel;

namespace RatingCalculator.Beta;

internal class BetaRatingResult<TEntity> : IRatingResult<TEntity> where TEntity : IEquatable<TEntity>
{
    private readonly IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> _distributions;
    private readonly StrengthProbabilityDistribution _default;

    public BetaRatingResult(IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> distributions)
    {
        _distributions = distributions;
        _default = new StrengthProbabilityDistribution();
    }

    public IRating Get(TEntity entity)
    {
        var distribution = _distributions.GetValueOrDefault(entity, _default);
        var others = _distributions
            .Where(kv => !kv.Key.Equals(kv))
            .Select(kv => kv.Value);

        return new BetaRating(distribution, others);
    }
}