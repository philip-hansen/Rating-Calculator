using RatingCalculator.Models;
using System.Collections.ObjectModel;

namespace RatingCalculator.Beta;

/// <summary>
/// Calculates beta ratings. (TODO: description)
/// </summary>
/// <typeparam name="TEntity">Type the entities are identified by</typeparam>
public class BetaRatingCalculator<TEntity> : IRatingCalculator<TEntity> where TEntity : IEquatable<TEntity>
{
    private readonly ExpectedResultCalculator _expectedResultCalculator;
    private readonly StrengthProbabilityDistribution _defaultDistribution;

    private readonly int _size;
    private readonly int _iterations;

    public BetaRatingCalculator(BetaRatingOptions options)
    {
        _size = options.Size;
        _iterations = options.Iterations;
        _expectedResultCalculator = 
            options.OptimizeForSpeed 
            ? new CachingExpectedResultCalculator(_size)
            : new ExpectedResultCalculator(_size);

        _defaultDistribution = new StrengthProbabilityDistribution(_size);
    }

    public BetaRatingCalculator() : this(new()) { }

    public IRatingResult<TEntity> CalculateRatings(IEnumerable<Game<TEntity>> games)
    {
        IEnumerable<TEntity> entities = games.Select(g => g.HomeEntity)
            .Union(games.Select(g => g.AwayEntity))
            .Distinct();

        ILookup<TEntity, ScheduledGame<TEntity>> schedules = GetSchedules(games);

        IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> currentRatings 
            = new Dictionary<TEntity, StrengthProbabilityDistribution>();

        for (int i = 0; i < _iterations; i++)
        {
            currentRatings = UpdateCurrentRatings(currentRatings, schedules, entities);
        }

        return new BetaRatingResult<TEntity>(currentRatings, schedules, _size);
    }

    private IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> UpdateCurrentRatings(
        IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> currentRatings,
        ILookup<TEntity, ScheduledGame<TEntity>> scheduledGames,
        IEnumerable<TEntity> entities)
    {
        IDictionary<TEntity, StrengthProbabilityDistribution> newDistributions 
            = new Dictionary<TEntity, StrengthProbabilityDistribution>();

        foreach (TEntity entity in entities)
        {
            IEnumerable<MyGame> myGames = scheduledGames[entity]
                .Select(sg => new MyGame(currentRatings.GetValueOrDefault(sg.Opponent, _defaultDistribution), sg.Result));

            newDistributions[entity] = new StrengthProbabilityDistribution(myGames, _expectedResultCalculator, _size);
        }

        return new ReadOnlyDictionary<TEntity, StrengthProbabilityDistribution>(newDistributions);
    }

    private static ILookup<TEntity, ScheduledGame<TEntity>> GetSchedules(IEnumerable<Game<TEntity>> games)
    {
        return games.Select(g => new
            {
                Me = g.HomeEntity,
                Opponent = g.AwayEntity,
                Result = g.Result.ToMyGameHomeResult(),
            })
            .Concat(games.Select(g => new
            {
                Me = g.AwayEntity,
                Opponent = g.HomeEntity,
                Result = g.Result.ToMyGameAwayResult(),
            }))
            .ToLookup(g => g.Me, g => new ScheduledGame<TEntity>(g.Opponent, g.Result));
    }
}