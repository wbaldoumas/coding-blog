using System.Globalization;

namespace Coding.Blog.Library.Extensions;

public static class DateTimeExtensions
{
    private const string _format = "D";

    private static readonly IFormatProvider _formatProvider = new CultureInfo("en-US");

    public static string ToPreformattedString(this DateTime source) => source.ToString(_format, _formatProvider);
}
