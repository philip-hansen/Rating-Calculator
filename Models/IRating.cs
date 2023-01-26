namespace RatingCalculator.Models;

public interface IRating
{
    double Mean();

    double Variance();

    double Mode();

    double ProbabilityOfChampion();

    double StrengthOfSchedule();
}