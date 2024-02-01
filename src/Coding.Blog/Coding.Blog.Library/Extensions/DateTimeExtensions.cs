using System.Globalization;

namespace Coding.Blog.Library.Extensions;

public static class DateTimeExtensions
{
    private const string _format = "D";

    /// <summary>
    ///     Formats the given date time in a unified way for the blog.
    /// </summary>
    /// <param name="source">The source date time to format.</param>
    /// <returns>The formatted date time.</returns>
    public static string ToPreformattedString(this DateTime source) => source.ToString(_format, CultureInfo.InvariantCulture);
}
