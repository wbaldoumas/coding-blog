namespace Coding.Blog.Utilities;

/// <summary>
///     Calculates estimated reading time.
/// </summary>
internal interface IReadTimeEstimator
{
    /// <summary>
    ///     Retrieves the estimated reading time of the given content.
    /// </summary>
    /// <param name="content">The content to estimate the reading time of.</param>
    /// <returns>The estimated reading time of the given content.</returns>
    public TimeSpan Estimate(string content);
}
