using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class BetaRatingResult<TEntity> : IRatingResult<TEntity> where TEntity : IEquatable<TEntity>
{
    private readonly IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> _distributions;
    private readonly StrengthProbabilityDistribution _default;
    private readonly int _size;

    public BetaRatingResult(IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> distributions, int size)
    {
        _distributions = distributions;
        _default = new StrengthProbabilityDistribution(size);
        _size = size;
    }

    public IRating Get(TEntity entity)
    {
        var distribution = _distributions.GetValueOrDefault(entity, _default);
        var others = _distributions
            .Where(kv => !kv.Key.Equals(entity))
            .Select(kv => kv.Value);

        return new BetaRating(distribution, others);
    }

    public IRating GetGroup(IEnumerable<TEntity> entities)
    {
        var allDistributions = _distributions.Select(kv => kv.Value);

        if (!entities.Any())
        {
            return new BetaRating(new(_size), allDistributions);
        }

        var distributions = entities.Select(e => _distributions.GetValueOrDefault(e, _default));

        return BetaRating.FromGroup(distributions, allDistributions);
    }
}