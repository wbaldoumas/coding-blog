using System.Globalization;

namespace Coding.Blog.Library.Extensions;

public static class DateTimeExtensions
{
    private const string _format = "D";

    public static string ToPreformattedString(this DateTime source) => source.ToString(_format, CultureInfo.InvariantCulture);
}
