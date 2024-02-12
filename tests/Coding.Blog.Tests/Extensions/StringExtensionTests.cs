using Coding.Blog.Library.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.Tests.Extensions;

[TestFixture]
internal sealed class StringExtensionTests
{
    [Test]
    public void ToDisplayTags_WithTags_ReturnsDisplayTags()
    {
        // arrange
        const string tags = "Tag1, Tag2, Tag3";

        // act
        var result = tags.ToDisplayTags();

        // assert
        result.Should().BeEquivalentTo(["Tag1", "Tag2", "Tag3"]);
    }

    [Test]
    public void ToDisplayTags_WithEmptyTags_ReturnsEmptyDisplayTags()
    {
        // arrange
        const string tags = "";

        // act
        var result = tags.ToDisplayTags();

        // assert
        result.Should().BeEmpty();
    }

    [Test]
    public void ToMarkupString_WithContent_ReturnsMarkupString()
    {
        // arrange
        const string content = "Sample content";

        // act
        var result = content.ToMarkupString();

        // assert
        result.Value.Should().Be(content);
    }

    [Test]
    public void ToMarkupString_WithEmptyContent_ReturnsEmptyMarkupString()
    {
        // arrange
        const string content = "";

        // act
        var result = content.ToMarkupString();

        // assert
        result.Value.Should().BeEmpty();
    }
}
