using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class BetaRatingResult<TEntity> : IRatingResult<TEntity> where TEntity : IEquatable<TEntity>
{
    private readonly IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> _distributions;
    private readonly ILookup<TEntity, ScheduledGame<TEntity>> _schedules;
    private readonly StrengthProbabilityDistribution _default;
    private readonly int _size;

    public BetaRatingResult(
        IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> distributions,
        ILookup<TEntity, ScheduledGame<TEntity>> schedules,
        int size)
    {
        _distributions = distributions;
        _default = new StrengthProbabilityDistribution(size);
        _schedules = schedules;
        _size = size;
    }

    public IRating Get(TEntity entity)
    {
        var distribution = _distributions.GetValueOrDefault(entity, _default);
        var others = _distributions
            .Where(kv => !kv.Key.Equals(entity))
            .Select(kv => kv.Value);

        var opponents = _schedules[entity]
            .Select(sg => _distributions.GetValueOrDefault(sg.Opponent, _default));

        return new BetaRating(distribution, others, opponents);
    }

    public IRating GetGroup(IEnumerable<TEntity> entities)
    {
        var distributions = entities.Select(e => _distributions.GetValueOrDefault(e, _default));

        var allDistributions = _distributions.Select(kv => kv.Value);

        var opponents = entities
            .SelectMany(e => _schedules[e])
            .Select(sg => _distributions.GetValueOrDefault(sg.Opponent, _default));

        if (!entities.Any())
        {
            return new BetaRating(new(_size), allDistributions, Enumerable.Empty<StrengthProbabilityDistribution>());
        }

        return BetaRating.FromGroup(distributions, allDistributions, opponents);
    }
}