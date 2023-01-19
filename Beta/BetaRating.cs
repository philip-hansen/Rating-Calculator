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

    public double Mean()
    {
        double total = 0;
        _probabilities.ForEach(s =>
        {
            total += s.NormalizedValue * _probabilities.Density(s);
        });
        
        return total;
    }

    public double Mode()
    {
        Strength maxStrength = new(0);
        double maxProbability = 0;

        _probabilities.ForEach(s =>
        {
            double density = _probabilities.Density(s);
            if (density > maxProbability)
            {
                maxStrength = s;
                maxProbability = density;
            }
        });

        return maxStrength.Value;
    }

    public double Variance()
    {
        return 0.0;
    }

    public double ProbabilityOfChampion()
    {
        return 0.0;
    }
}

