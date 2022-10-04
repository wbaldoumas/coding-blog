namespace Coding.Blog.Engine.Utilities;

internal sealed class ReadTimeEstimator : IReadTimeEstimator
{
    private const double AverageWordsPerMinute = 250.00;

    private readonly IStringSanitizer _sanitizer;

    public ReadTimeEstimator(IStringSanitizer sanitizer) => _sanitizer = sanitizer;

    public TimeSpan Estimate(string content)
    {
        var sanitizedString = _sanitizer.Sanitize(content);
        var wordCount = sanitizedString.Split(' ').Length;

        var estimatedReadingMinutes = wordCount / AverageWordsPerMinute;

        return estimatedReadingMinutes < 1
            ? TimeSpan.FromMinutes(1)
            : TimeSpan.FromMinutes(Math.Round(estimatedReadingMinutes, MidpointRounding.AwayFromZero));
    }
}
