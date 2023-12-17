namespace Coding.Blog.Shared.Utilities;

internal sealed class ReadTimeEstimator(IStringSanitizer sanitizer) : IReadTimeEstimator
{
    private const double AverageWordsPerMinute = 250.00;

    private const int MinimumReadingTime = 1;

    public TimeSpan Estimate(string content)
    {
        var sanitizedString = sanitizer.Sanitize(content);
        var wordCount = sanitizedString.Split(' ').Length;

        var estimatedReadingMinutes = wordCount / AverageWordsPerMinute;

        return estimatedReadingMinutes < MinimumReadingTime
            ? TimeSpan.FromMinutes(MinimumReadingTime)
            : TimeSpan.FromMinutes(Math.Round(estimatedReadingMinutes, MidpointRounding.AwayFromZero));
    }
}
