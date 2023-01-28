namespace RatingCalculator.Beta;

/// <summary>
/// Options for the beta rating calculator
/// </summary>
public class BetaRatingOptions
{
    /// <summary>
    /// Number of different strength possibilities.
    /// The higher the number, the more objective and accurate the result,
    /// but will require more computational power and memory.
    /// </summary>
    public int Size { get; init; } = 1001;

    /// <summary>
    /// Number of times to iterate the algorithm to allow it to converge.
    /// The higher the number, the more objective and accurate the result,
    /// but will require more computational power.
    /// </summary>
    public int Iterations { get; init; } = 5;

    /// <summary>
    /// Use caching to make the algorithm go faster.
    /// When true, more memory will be used.
    /// </summary>
    public bool OptimizeForSpeed { get; init; } = true;
}