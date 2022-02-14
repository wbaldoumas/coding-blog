using System;
using System.Data;
using System.Linq;
using Coding.Blog.Engine.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Utilities;

[TestFixture]
public class ReadTimeEstimatorTests
{
    private IStringSanitizer _mockStringSanitizer;

    [SetUp]
    public void SetUp()
    {
        _mockStringSanitizer = Substitute.For<IStringSanitizer>();
    }

    [Test]
    [TestCase(1, 1)]
    [TestCase(250, 1)]
    [TestCase(2500, 10)]
    public void ReadTimeEstimator_generates_expected_estimate(int wordCount, int expectedReadTimeMinutes)
    {
        // arrange
        var content = string.Join(' ', Enumerable.Range(0, wordCount).Select(_ => "hello"));

        _mockStringSanitizer
            .Sanitize(content)
            .Returns(content);

        var expectedReadTime = TimeSpan.FromMinutes(expectedReadTimeMinutes);

        var readTimeEstimator = new ReadTimeEstimator(_mockStringSanitizer);

        // act
        var estimatedReadTime = readTimeEstimator.Estimate(content);

        // assert
        estimatedReadTime.Should().Be(expectedReadTime);
    }
}
