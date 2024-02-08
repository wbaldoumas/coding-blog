using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Exceptions;
using Coding.Blog.Library.State;
using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.Tests.State;

[TestFixture]
internal sealed class ApplicationStateTests
{
    [Test]
    public void WhenBooksAreSet_ThenTheyAreInState()
    {
        // arrange
        ApplicationState.SetState(ExpectedBooks, Book.Key);

        // act
        var actualBooks = ApplicationState.GetState<Book>(Book.Key);

        // assert
        actualBooks.Should().BeEquivalentTo(ExpectedBooks);
    }

    private static readonly Book ExpectedBook = new(
        Title: "some book",
        Author: "some author",
        DatePublished: DateTime.UtcNow,
        PurchaseUrl: "some url",
        Image: new Image(
            Url: "some image url",
            ImgixUrl: "some imgix url"
        ),
        Content: "some content"
    );

    private static readonly List<Book> ExpectedBooks = [ExpectedBook];

    [Test]
    public void WhenProjectsAreSet_ThenTheyAreInState()
    {
        // arrange
        ApplicationState.SetState(ExpectedProjects, Project.Key);

        // act
        var actualProjects = ApplicationState.GetState<Project>(Project.Key);

        // assert
        actualProjects.Should().BeEquivalentTo(ExpectedProjects);
    }

    private static readonly Project ExpectedProject = new(
        Title: "some project",
        Description: "some description",
        ProjectUrl: "some url",
        Image: new Image(
            Url: "some image url",
            ImgixUrl: "some imgix url"
        ),
        Rank: 1,
        Tags: "some,tags"
    );

    private static readonly List<Project> ExpectedProjects = [ExpectedProject];

    [Test]
    public void WhenPostsAreSet_ThenTheyAreInState()
    {
        // arrange
        ApplicationState.SetState(ExpectedPosts, Post.Key);

        // act
        var actualPosts = ApplicationState.GetState<Post>(Post.Key);

        // assert
        actualPosts.Should().BeEquivalentTo(ExpectedPosts);
    }

    private static readonly Post ExpectedPost = new(
        Id: "some id",
        Slug: "some-slug",
        Title: "some post",
        Description: "some description",
        DatePublished: DateTime.UtcNow,
        Image: new Image(
            Url: "some image url",
            ImgixUrl: "some imgix url"
        ),
        Content: "some content",
        Tags: "some,tags",
        ReadingTime: TimeSpan.MaxValue
    );

    private static readonly List<Post> ExpectedPosts = [ExpectedPost];

    [Test]
    public void WhenInvalidKeyIsUsedForSetState_ThenInvalidApplicationStateKeyExceptionIsThrown()
    {
        // act
        var act = () => ApplicationState.SetState(ExpectedPosts, "invalid key");

        // assert
        act.Should().Throw<InvalidApplicationStateKeyException>();
    }

    [Test]
    public void WhenInvalidKeyIsUsedForGetState_ThenInvalidApplicationStateKeyExceptionIsThrown()
    {
        // act
        var act = () => ApplicationState.GetState<Post>("invalid key");

        // assert
        act.Should().Throw<InvalidApplicationStateKeyException>();
    }
}
