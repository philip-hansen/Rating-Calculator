using RatingCalculator.Models;
using System.Collections.ObjectModel;

namespace RatingCalculator.Beta;

public class BetaRatingCalculator<TEntity> : IRatingCalculator<TEntity> where TEntity : IEquatable<TEntity>
{
    private readonly IExpectedResultCalculator _expectedResultCalculator;
    private readonly StrengthProbabilityDistribution _defaultDistribution;

    private readonly int _size;

    public BetaRatingCalculator(int size)
    {
        var expectedResultCalculator = new ComputingExpectedResultCalculator();
        _expectedResultCalculator = expectedResultCalculator;
        _defaultDistribution = new StrengthProbabilityDistribution();

        _size = size;
    }

    public IRatingResult<TEntity> CalculateRatings(IEnumerable<Game<TEntity>> games)
    {
        IEnumerable<TEntity> entities = games.Select(g => g.HomeEntity)
            .Union(games.Select(g => g.AwayEntity))
            .Distinct();

        ILookup<TEntity, ScheduledGame<TEntity>> schedules = GetSchedules(games);

        IReadOnlyDictionary<TEntity, StrengthProbabilityDistribution> currentRatings 
            = new Dictionary<TEntity, StrengthProbabilityDistribution>();

        // Hardcode 5 for now
        for (int i = 0; i < 5; i++)
        {
            currentRatings = UpdateCurrentRatings(currentRatings, schedules, entities);
        }

        return new BetaRatingResult<TEntity>(currentRatings);
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