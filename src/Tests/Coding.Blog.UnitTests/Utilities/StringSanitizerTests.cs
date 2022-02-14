using Coding.Blog.Engine.Utilities;
using FluentAssertions;
using Markdig;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Utilities;

[TestFixture]
public class StringSanitizerTests
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
        sanitizedString.Should().Be(SanitizedString);
    }

    private const string MarkdownString = @"
# This is a header

Here is some other content with a [link](https://google.com).

## Code example sub-header

```
var foo = new Bar();
var bar = foo.Bar();
```
";

    private const string SanitizedString = @"This is a header
Here is some other content with a link.
Code example sub-header
var foo = new Bar();
var bar = foo.Bar();
";
}