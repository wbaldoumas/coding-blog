using System.Diagnostics.CodeAnalysis;
using Google.Protobuf.WellKnownTypes;

namespace Coding.Blog.Engine.Extensions;

/// <summary>
///     Utility extensions for the <see cref="Timestamp"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TimestampExtensions
{
    /// <summary>
    ///     Generates a "short" date string from the given <see cref="Timestamp"/>.
    /// </summary>
    /// <param name="timestamp">The <see cref="Timestamp"/> to generate the short date string with</param>
    /// <returns>The short date string generated from the <see cref="Timestamp"/></returns>
    public static string ToShortDateString(this Timestamp timestamp) => timestamp.ToDateTime().ToShortDateString();
}