using Coding.Blog.Library.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using NUnit.Framework;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;


namespace Coding.Blog.Tests.Utilities;

[TestFixture]
public sealed class MapperTests
{
    private IReadTimeEstimator _mockReadTimeEstimator = null!;

    private Mapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _mockReadTimeEstimator = Substitute.For<IReadTimeEstimator>();

        _mapper = new Mapper(_mockReadTimeEstimator);
    }

    [Test]
    public void Map_CosmicBookToBook_ReturnsBook()
    {
        // arrange
        var cosmicBook = new CosmicBook(
            Title: "Title",
            DatePublished: DateTime.Now.ToUniversalTime(),
            Metadata: new CosmicBookMetadata(
                PurchaseUrl: "PurchaseUrl",
                Image: new CosmicImage(
                    Url: "Url",
                    ImgixUrl: "ImgixUrl"
                ),
                Content: "Content",
                Author: "Author"
            )
        );

        // act
        var book = _mapper.Map<CosmicBook, Book>(cosmicBook);

        // assert
        book.Title.Should().Be(cosmicBook.Title);
        book.Content.Should().Be(cosmicBook.Metadata.Content);
        book.Author.Should().Be(cosmicBook.Metadata.Author);
        book.DatePublished.Should().Be(cosmicBook.DatePublished);
        book.PurchaseUrl.Should().Be(cosmicBook.Metadata.PurchaseUrl);
        book.Image.Url.Should().Be(cosmicBook.Metadata.Image.Url);
        book.Image.ImgixUrl.Should().Be(cosmicBook.Metadata.Image.ImgixUrl);
    }

    [Test]
    public void Map_CosmicBookToProtoBook_ReturnsProtoBook()
    {
        // arrange
        var cosmicBook = new CosmicBook(
            Title: "Title",
            DatePublished: DateTime.Now.ToUniversalTime(),
            Metadata: new CosmicBookMetadata(
                PurchaseUrl: "PurchaseUrl",
                Image: new CosmicImage(
                    Url: "Url",
                    ImgixUrl: "ImgixUrl"
                ),
                Content: "Content",
                Author: "Author"
            )
        );

        // act
        var protoBook = _mapper.Map<CosmicBook, ProtoBook>(cosmicBook);

        // assert
        protoBook.Title.Should().Be(cosmicBook.Title);
        protoBook.Content.Should().Be(cosmicBook.Metadata.Content);
        protoBook.DatePublished.Should().Be(cosmicBook.DatePublished.ToTimestamp());
        protoBook.PurchaseUrl.Should().Be(cosmicBook.Metadata.PurchaseUrl);
        protoBook.Image.Url.Should().Be(cosmicBook.Metadata.Image.Url);
        protoBook.Image.ImgixUrl.Should().Be(cosmicBook.Metadata.Image.ImgixUrl);
    }

    [Test]
    public void Map_ProtoBookToBook_ReturnsBook()
    {
        // arrange
        var protoBook = new ProtoBook
        {
            Title = "Title",
            Content = "Content",
            DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
            PurchaseUrl = "PurchaseUrl",
            Image = new Image
            {
                Url = "Url",
                ImgixUrl = "ImgixUrl"
            }
        };

        // act
        var book = _mapper.Map<ProtoBook, Book>(protoBook);

        // assert
        book.Title.Should().Be(protoBook.Title);
        book.Content.Should().Be(protoBook.Content);
        book.Author.Should().Be(protoBook.Author);
        book.DatePublished.Should().Be(protoBook.DatePublished.ToDateTime());
        book.PurchaseUrl.Should().Be(protoBook.PurchaseUrl);
        book.Image.Url.Should().Be(protoBook.Image.Url);
        book.Image.ImgixUrl.Should().Be(protoBook.Image.ImgixUrl);
    }

    [Test]
    public void Map_CosmicPostToPost_ReturnsPost()
    {
        // arrange
        var readingTime = TimeSpan.FromMinutes(1);

        _mockReadTimeEstimator.Estimate(Arg.Any<string>()).Returns(readingTime);

        var cosmicPost = new CosmicPost(
            Title: "Title",
            Slug: "Slug",
            DatePublished: DateTime.Now.ToUniversalTime(),
            Metadata: new CosmicPostMetadata(
                Markdown: "Markdown",
                Tags: "Tag1,Tag2",
                Image: new CosmicImage(
                    Url: "Url",
                    ImgixUrl: "ImgixUrl"
                )
            )
        );

        // act
        var post = _mapper.Map<CosmicPost, Post>(cosmicPost);

        // assert
        post.Title.Should().Be(cosmicPost.Title);
        post.Slug.Should().Be(cosmicPost.Slug);
        post.DatePublished.Should().Be(cosmicPost.DatePublished);
        post.Content.Should().Be(cosmicPost.Metadata.Markdown);
        post.Tags.Should().Be(cosmicPost.Metadata.Tags);
        post.Image.Url.Should().Be(cosmicPost.Metadata.Image.Url);
        post.Image.ImgixUrl.Should().Be(cosmicPost.Metadata.Image.ImgixUrl);
        post.ReadingTime.Should().Be(readingTime);
    }

    [Test]
    public void Map_CosmicPostToProtoPost_ReturnsProtoPost()
    {
        // arrange
        var readingTime = TimeSpan.FromMinutes(1);

        _mockReadTimeEstimator.Estimate(Arg.Any<string>()).Returns(readingTime);

        var cosmicPost = new CosmicPost(
            Title: "Title",
            Slug: "Slug",
            DatePublished: DateTime.Now.ToUniversalTime(),
            Metadata: new CosmicPostMetadata(
                Markdown: "Markdown",
                Tags: "Tag1,Tag2",
                Image: new CosmicImage(
                    Url: "Url",
                    ImgixUrl: "ImgixUrl"
                )
            )
        );

        // act
        var protoPost = _mapper.Map<CosmicPost, ProtoPost>(cosmicPost);

        // assert
        protoPost.Title.Should().Be(cosmicPost.Title);
        protoPost.Slug.Should().Be(cosmicPost.Slug);
        protoPost.DatePublished.Should().Be(cosmicPost.DatePublished.ToTimestamp());
        protoPost.Content.Should().Be(cosmicPost.Metadata.Markdown);
        protoPost.Tags.Should().Be(cosmicPost.Metadata.Tags);
        protoPost.Image.Url.Should().Be(cosmicPost.Metadata.Image.Url);
        protoPost.Image.ImgixUrl.Should().Be(cosmicPost.Metadata.Image.ImgixUrl);
        protoPost.ReadingTime.Should().Be(readingTime.ToDuration());
    }

    [Test]
    public void Map_ProtoPostToPost_ReturnsPost()
    {
        // arrange
        var protoPost = new ProtoPost
        {
            Title = "Title",
            Slug = "Slug",
            ReadingTime = TimeSpan.FromMinutes(1).ToDuration(),
            DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
            Content = "Content",
            Tags = "Tag1,Tag2",
            Image = new Image
            {
                Url = "Url",
                ImgixUrl = "ImgixUrl"
            }
        };

        // act
        var post = _mapper.Map<ProtoPost, Post>(protoPost);

        // assert
        post.Title.Should().Be(protoPost.Title);
        post.Slug.Should().Be(protoPost.Slug);
        post.ReadingTime.Should().Be(protoPost.ReadingTime.ToTimeSpan());
        post.DatePublished.Should().Be(protoPost.DatePublished.ToDateTime());
        post.Content.Should().Be(protoPost.Content);
        post.Tags.Should().Be(protoPost.Tags);
        post.Image.Url.Should().Be(protoPost.Image.Url);
        post.Image.ImgixUrl.Should().Be(protoPost.Image.ImgixUrl);
    }

    [Test]
    public void Map_CosmicProjectToProject_ReturnsProject()
    {
        // arrange
        var cosmicProject = new CosmicProject(
            Title: "Title",
            Metadata: new CosmicProjectMetadata(
                Description: "Description",
                Rank: 1,
                GitHubUrl: "GitHubUrl",
                Tags: "Tag1,Tag2",
                Image: new CosmicImage(
                    Url: "Url",
                    ImgixUrl: "ImgixUrl"
                )
            )
        );

        // act
        var project = _mapper.Map<CosmicProject, Project>(cosmicProject);

        // assert
        project.Title.Should().Be(cosmicProject.Title);
        project.Description.Should().Be(cosmicProject.Metadata.Description);
        project.ProjectUrl.Should().Be(cosmicProject.Metadata.GitHubUrl);
        project.Rank.Should().Be(cosmicProject.Metadata.Rank);
        project.Tags.Should().Be(cosmicProject.Metadata.Tags);
        project.Image.Url.Should().Be(cosmicProject.Metadata.Image.Url);
        project.Image.ImgixUrl.Should().Be(cosmicProject.Metadata.Image.ImgixUrl);
    }

    [Test]
    public void Map_CosmicProjectToProtoProject_ReturnsProtoProject()
    {
        // arrange
        var cosmicProject = new CosmicProject(
            Title: "Title",
            Metadata: new CosmicProjectMetadata(
                Description: "Description",
                Rank: 1,
                GitHubUrl: "GitHubUrl",
                Tags: "Tag1,Tag2",
                Image: new CosmicImage(
                    Url: "Url",
                    ImgixUrl: "ImgixUrl"
                )
            )
        );

        // act
        var protoProject = _mapper.Map<CosmicProject, ProtoProject>(cosmicProject);

        // assert
        protoProject.Title.Should().Be(cosmicProject.Title);
        protoProject.Description.Should().Be(cosmicProject.Metadata.Description);
        protoProject.ProjectUrl.Should().Be(cosmicProject.Metadata.GitHubUrl);
        protoProject.Rank.Should().Be(cosmicProject.Metadata.Rank);
        protoProject.Tags.Should().Be(cosmicProject.Metadata.Tags);
        protoProject.Image.Url.Should().Be(cosmicProject.Metadata.Image.Url);
        protoProject.Image.ImgixUrl.Should().Be(cosmicProject.Metadata.Image.ImgixUrl);
    }

    [Test]
    public void Map_ProtoProjectToProject_ReturnsProject()
    {
        // arrange
        var protoProject = new ProtoProject
        {
            Title = "Title",
            Description = "Description",
            ProjectUrl = "ProjectUrl",
            Rank = 1,
            Tags = "Tag1,Tag2",
            Image = new Image
            {
                Url = "Url",
                ImgixUrl = "ImgixUrl"
            }
        };

        // act
        var project = _mapper.Map<ProtoProject, Project>(protoProject);

        // assert
        project.Title.Should().Be(protoProject.Title);
        project.Description.Should().Be(protoProject.Description);
        project.ProjectUrl.Should().Be(protoProject.ProjectUrl);
        project.Rank.Should().Be(protoProject.Rank);
        project.Tags.Should().Be(protoProject.Tags);
        project.Image.Url.Should().Be(protoProject.Image.Url);
        project.Image.ImgixUrl.Should().Be(protoProject.Image.ImgixUrl);
    }
}
