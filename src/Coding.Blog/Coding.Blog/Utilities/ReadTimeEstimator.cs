namespace Coding.Blog.Utilities;

/// <summary>
///     A service for estimating the read time of a given string.
/// </summary>
/// <param name="sanitizer">A <see cref="IStringSanitizer"/> for sanitizing the string.</param>
internal sealed class ReadTimeEstimator(
    IStringSanitizer sanitizer, 
    double averageWordsPerMinute = 250.00
) : IReadTimeEstimator
{
    private const int MinimumReadingTimeMinutes = 1;

    public TimeSpan Estimate(string content)
    {
        var sanitizedString = sanitizer.Sanitize(content);
        var wordCount = sanitizedString.Split(' ').Length;

        var estimatedReadingMinutes = wordCount / averageWordsPerMinute;

        return estimatedReadingMinutes < MinimumReadingTimeMinutes
            ? TimeSpan.FromMinutes(MinimumReadingTimeMinutes)
            : TimeSpan.FromMinutes(Math.Round(estimatedReadingMinutes, MidpointRounding.AwayFromZero));
    }
}
