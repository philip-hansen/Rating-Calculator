using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class BetaRating : IRating
{
    private readonly StrengthProbabilityDistribution _probabilities;
    private readonly IEnumerable<StrengthProbabilityDistribution> _otherEntities;
    private readonly IEnumerable<StrengthProbabilityDistribution> _opponents;

    public BetaRating(
        StrengthProbabilityDistribution strengthProbabilityDistribution,
        IEnumerable<StrengthProbabilityDistribution> otherEntities,
        IEnumerable<StrengthProbabilityDistribution> opponents)
    {
        _probabilities = strengthProbabilityDistribution;
        _otherEntities = otherEntities;
        _opponents = opponents;
    }

    public double Mean() => _probabilities.Mean();

    public double Mode() => _probabilities.Mode();

    public double Variance() => _probabilities.Variance();

    public double Median() => _probabilities.Median();

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
            .Select(o => 1 - o.ExpectedResultAgainstAverage)
            .Average();

    public static BetaRating FromGroup(
        IEnumerable<StrengthProbabilityDistribution> groupMembers,
        IEnumerable<StrengthProbabilityDistribution> allEntities,
        IEnumerable<StrengthProbabilityDistribution> opponents) => 
            new(StrengthProbabilityDistribution.FromGroup(groupMembers), 
                allEntities.Except(groupMembers), 
                opponents);
}