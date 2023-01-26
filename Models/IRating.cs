namespace RatingCalculator.Models;

/// <summary>
/// Represents a calculated rating for a given entity.
/// </summary>
public interface IRating
{
    /// <summary>
    /// Calculate the expected value of the rating.
    /// For non-probabilistic ratings, this will be the point value.
    /// </summary>
    /// <returns>Expected value</returns>
    double Mean();

    /// <summary>
    /// Calculate the variance of the rating.
    /// For non-probabilistic ratings, this will be 0.
    /// </summary>
    /// <returns>Variance</returns>
    double Variance();

    /// <summary>
    /// Calculate the most likely value of the rating.
    /// For non-probabilistic ratings, this will be the point value (and equivalent to mean).
    /// </summary>
    /// <returns>Mode</returns>
    double Mode();

    /// <summary>
    /// Calculate the probability that the value of no other rating in the system is higher than this one.
    /// Only other entities that have participated in at least one game are considered.
    /// For non-probabilistic ratings, this will be 100% if it is the highest, and 0% otherwise.
    /// </summary>
    /// <returns>Probability of Champion</returns>
    double ProbabilityOfChampion();

    /// <summary>
    /// Calculate the expected record that an average (or some anchor) entity would have against the schedule of this one.
    /// </summary>
    /// <returns>Strength of Schedule</returns>
    double StrengthOfSchedule();
}