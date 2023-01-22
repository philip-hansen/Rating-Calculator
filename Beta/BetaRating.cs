using RatingCalculator.Models;

namespace RatingCalculator.Beta;

internal class BetaRating : IRating
{
    private readonly StrengthProbabilityDistribution _probabilities;
    private readonly IEnumerable<StrengthProbabilityDistribution> _otherEntities;

    public BetaRating(
        StrengthProbabilityDistribution strengthProbabilityDistribution,
        IEnumerable<StrengthProbabilityDistribution> otherEntities)
    {
        _probabilities = strengthProbabilityDistribution;
        _otherEntities = otherEntities;
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

    public static BetaRating FromGroup(
        IEnumerable<StrengthProbabilityDistribution> groupMembers,
        IEnumerable<StrengthProbabilityDistribution> allEntities) =>
            new(StrengthProbabilityDistribution.FromGroup(groupMembers), allEntities.Except(groupMembers));
}