namespace RatingCalculator.Beta;

internal class BetaRating
{
    private readonly BetaRatingProbabilities _probabilities;

    public BetaRating()
    {
        _probabilities = new();
    }

    private double ExpectedResult(BetaRating other)
    {
        // TODO: Inject
        IExpectedResultCalculator calculator = new ComputingExpectedResultCalculator();
        
        double result = 0;

        _probabilities.ForEach(myStrength =>
        {
            double myStrengthProbability = _probabilities.Get(myStrength);

            other._probabilities.ForEach(yourStrength =>
            {
                double yourStrengthProbability = other._probabilities.Get(yourStrength);
                double totalProbability = myStrengthProbability * yourStrengthProbability;

                result += totalProbability * calculator.CalculateStrengthExpectedResult(myStrength, yourStrength);
            });
        });

        return result;
    }
}

