using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class BetaRatingResult<TEntity> : IRatingResult<TEntity> where TEntity : IEquatable<TEntity>
{
    private readonly IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> _distributions;
    private readonly StrengthProbabilityDistribution _default;

    public BetaRatingResult(IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> distributions, int size)
    {
        _distributions = distributions;
        _default = new StrengthProbabilityDistribution(size);
    }

    public IRating Get(TEntity entity)
    {
        var distribution = _distributions.GetValueOrDefault(entity, _default);
        var others = _distributions
            .Where(kv => !kv.Key.Equals(kv))
            .Select(kv => kv.Value);

        return new BetaRating(distribution, others);
    }

    public IRating GetGroup(IEnumerable<TEntity> entities)
    {
        if (!entities.Any())
        {
            throw new ArgumentException("At least one entity is required in group", nameof(entities));
        }

        var distributions = entities.Select(e => _distributions.GetValueOrDefault(e, _default));

        var allDistributions = _distributions.Select(kv => kv.Value);

        return BetaRating.FromGroup(distributions, allDistributions);
    }
}