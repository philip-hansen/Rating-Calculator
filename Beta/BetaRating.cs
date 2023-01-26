using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class BetaRating<TEntity> : IRating
{
    private readonly StrengthProbabilityDistribution _probabilities;
    private readonly IEnumerable<StrengthProbabilityDistribution> _otherEntities;
    private readonly IEnumerable<StrengthProbabilityDistribution> _opponents;
    private readonly ExpectedResultCalculator _calculator;
    private readonly int _size;

    public BetaRating(
        StrengthProbabilityDistribution strengthProbabilityDistribution,
        IEnumerable<StrengthProbabilityDistribution> otherEntities,
        IEnumerable<StrengthProbabilityDistribution> opponents,
        ExpectedResultCalculator calculator,
        int size)
    {
        _probabilities = strengthProbabilityDistribution;
        _otherEntities = otherEntities;
        _opponents = opponents;
        _calculator = calculator;
        _size = size;
    }

    public double Mean() => _probabilities.Mean();

    public double Mode() => _probabilities.Mode();

    public double Variance() => _probabilities.Variance();

    public double ProbabilityOfChampion()
    {
        double totalProbability = 0.0;

        _probabilities.ForEach(strength =>
        {
            double probOfChampion = _probabilities.Density(strength);
            foreach (var opponent in _otherEntities)
            {
                probOfChampion *= opponent.Cumulative(strength);
            }

            totalProbability += probOfChampion;
        });

        return totalProbability;
    }

    public double StrengthOfSchedule() =>
        _opponents
            .Select(o => 1 - _calculator.CalculateExpectedResult(new Strength(_size / 2, _size), o))
            .Average();

    public static BetaRating<TEntity> FromGroup(
        IEnumerable<StrengthProbabilityDistribution> groupMembers,
        IEnumerable<StrengthProbabilityDistribution> allEntities,
        IEnumerable<StrengthProbabilityDistribution> opponents,
        ExpectedResultCalculator calculator,
        int size) => 
            new(
                StrengthProbabilityDistribution.FromGroup(groupMembers), 
                allEntities.Except(groupMembers), 
                opponents, 
                calculator, 
                size);
}