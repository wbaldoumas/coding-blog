using System.Globalization;
using Coding.Blog.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Utilities;

[TestFixture]
internal sealed class ReadTimeEstimatorTests
{
    private IStringSanitizer _mockStringSanitizer = null!;

    private ReadTimeEstimator _readTimeEstimator = null!;

    private const double AverageWordsPerMinute = 100;

    [SetUp]
    public void SetUp()
    {
        _mockStringSanitizer = Substitute.For<IStringSanitizer>();
        _readTimeEstimator = new ReadTimeEstimator(_mockStringSanitizer, AverageWordsPerMinute);
    }

    [Test]
    [TestCase(3, 3)]
    [TestCase(0, 1)]
    public void WhenReadTimeEstimatorEstimatesReadingTime_ThenItIsAccurate(
        int wordCountMultiplier,
        int expectedReadingTime)
    {
        // arrange
        var words = Enumerable
            .Repeat(1, (int)AverageWordsPerMinute * wordCountMultiplier)
            .Select((_, value) => value.ToString(CultureInfo.InvariantCulture));

        var content = string.Join(' ', words);

        _mockStringSanitizer.Sanitize(Arg.Any<string>()).Returns(content);

        // act
        var readingTime = _readTimeEstimator.Estimate(content);

        // assert
        readingTime.Should().BeCloseTo(
            TimeSpan.FromMinutes(expectedReadingTime),
            TimeSpan.FromSeconds(10)
        );
    }
}
