using Coding.Blog.Library.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.Tests.Extensions;

[TestFixture]
internal sealed class DateTimeExtensionsTests
{
    [Test]
    public void ToPreformattedString_WithDateTime_ReturnsPreformattedString()
    {
        // arrange
        var dateTime = new DateTime(2024, 1, 11);

        // act
        var result = dateTime.ToPreformattedString();

        // assert
        result.Should().Be("Thursday, 11 January 2024");
    }
}
