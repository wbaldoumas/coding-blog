using Coding.Blog.Engine.Utilities;
using FluentAssertions;
using Markdig;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Utilities;

[TestFixture]
internal sealed class StringSanitizerTests
{
    [Test]
    public void StringSanitizer_generates_expected_string()
    {
        // arrange
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var sanitizer = new StringSanitizer(pipeline);

        // act
        var sanitizedString = sanitizer.Sanitize(MarkdownString);

        // assert
        AssertEqualIgnoreNewLines(ExpectedSanitizedString, sanitizedString);
    }

    public static void AssertEqualIgnoreNewLines(string expected, string actual)
    {
        var normalizedExpected = expected.Replace("\r\n", "\n", StringComparison.OrdinalIgnoreCase).Replace('\r', '\n');
        var normalizedActual = actual.Replace("\r\n", "\n", StringComparison.OrdinalIgnoreCase).Replace('\r', '\n');

        normalizedExpected.Should().Be(normalizedActual);
    }

    private const string MarkdownString = """
                                          # This is a header

                                          Here is some other content with a [link](https://google.com).

                                          ## Code example sub-header

                                          ```
                                          var foo = new Bar();
                                          var bar = foo.Bar();
                                          ```
                                          """;

    private const string ExpectedSanitizedString = """
                                                   This is a header
                                                   Here is some other content with a link.
                                                   Code example sub-header
                                                   var foo = new Bar();
                                                   var bar = foo.Bar();

                                                   """;
}