using Coding.Blog.DataTransfer;
using Coding.Blog.Utilities;
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
internal sealed class ServerMapperTests
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
        AssertExpectedBookData(cosmicBook, book);
    }

    [Test]
    public void Map_CosmicBooksToBooks_ReturnsBooks()
    {
        // arrange
        var cosmicBooks = new List<CosmicBook>
        {
            new(
                Title: "Title 1",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicBookMetadata(
                    PurchaseUrl: "PurchaseUrl 1",
                    Image: new CosmicImage(
                        Url: "Url 1",
                        ImgixUrl: "ImgixUrl 1"
                    ),
                    Content: "Content 1",
                    Author: "Author 1"
                )
            ),
            new(
                Title: "Title 2",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicBookMetadata(
                    PurchaseUrl: "PurchaseUrl 2",
                    Image: new CosmicImage(
                        Url: "Url 2",
                        ImgixUrl: "ImgixUrl 2"
                    ),
                    Content: "Content 2",
                    Author: "Author 2"
                )
            )
        };

        // act
        var books = _mapper.Map<CosmicBook, Book>(cosmicBooks).ToList();

        // assert
        books.Should().HaveCount(2);

        AssertExpectedBookData(cosmicBooks[0], books[0]);
        AssertExpectedBookData(cosmicBooks[1], books[1]);
    }

    private static void AssertExpectedBookData(CosmicBook expected, Book actual)
    {
        actual.Title.Should().Be(expected.Title);
        actual.Content.Should().Be(expected.Metadata.Content);
        actual.Author.Should().Be(expected.Metadata.Author);
        actual.DatePublished.Should().Be(expected.DatePublished);
        actual.PurchaseUrl.Should().Be(expected.Metadata.PurchaseUrl);
        actual.Image.Url.Should().Be(expected.Metadata.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Metadata.Image.ImgixUrl);
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
        AssertExpectedBookData(cosmicBook, protoBook);
    }

    [Test]
    public void Map_CosmicBooksToProtoBooks_ReturnsProtoBooks()
    {
        // arrange
        var cosmicBooks = new List<CosmicBook>
        {
            new(
                Title: "Title 1",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicBookMetadata(
                    PurchaseUrl: "PurchaseUrl 1",
                    Image: new CosmicImage(
                        Url: "Url 1",
                        ImgixUrl: "ImgixUrl 1"
                    ),
                    Content: "Content 1",
                    Author: "Author 1"
                )
            ),
            new(
                Title: "Title 2",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicBookMetadata(
                    PurchaseUrl: "PurchaseUrl 2",
                    Image: new CosmicImage(
                        Url: "Url 2",
                        ImgixUrl: "ImgixUrl 2"
                    ),
                    Content: "Content 2",
                    Author: "Author 2"
                )
            )
        };

        // act
        var protoBooks = _mapper.Map<CosmicBook, ProtoBook>(cosmicBooks).ToList();

        // assert
        protoBooks.Should().HaveCount(2);

        AssertExpectedBookData(cosmicBooks[0], protoBooks[0]);
        AssertExpectedBookData(cosmicBooks[1], protoBooks[1]);
    }

    private static void AssertExpectedBookData(CosmicBook expected, ProtoBook actual)
    {
        actual.Title.Should().Be(expected.Title);
        actual.Content.Should().Be(expected.Metadata.Content);
        actual.DatePublished.Should().Be(expected.DatePublished.ToTimestamp());
        actual.PurchaseUrl.Should().Be(expected.Metadata.PurchaseUrl);
        actual.Image.Url.Should().Be(expected.Metadata.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Metadata.Image.ImgixUrl);
    }

    [Test]
    public void Map_CosmicPostToPost_ReturnsPost()
    {
        // arrange
        var readingTime = TimeSpan.FromMinutes(1);

        _mockReadTimeEstimator.Estimate(Arg.Any<string>()).Returns(readingTime);

        var cosmicPost = new CosmicPost(
            Id: "Id",
            Title: "Title",
            Slug: "Slug",
            DatePublished: DateTime.Now.ToUniversalTime(),
            Metadata: new CosmicPostMetadata(
                Description: "Description",
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
        AssertExpectedPostData(cosmicPost, post);
    }

    [Test]
    public void Map_CosmicPostsToPosts_ReturnsPosts()
    {
        // arrange
        var readingTime = TimeSpan.FromMinutes(1);

        _mockReadTimeEstimator.Estimate(Arg.Any<string>()).Returns(readingTime);

        var cosmicPosts = new List<CosmicPost>
        {
            new(
                Id: "Id 1",
                Title: "Title 1",
                Slug: "Slug 1",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicPostMetadata(
                    Description: "Description 1",
                    Markdown: "Markdown 1",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 1",
                        ImgixUrl: "ImgixUrl 1"
                    )
                )
            ),
            new(
                Id: "Id 2",
                Title: "Title 2",
                Slug: "Slug 2",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicPostMetadata(
                    Description: "Description 2",
                    Markdown: "Markdown 2",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 2",
                        ImgixUrl: "ImgixUrl 2"
                    )
                )
            )
        };

        // act
        var posts = _mapper.Map<CosmicPost, Post>(cosmicPosts).ToList();

        // assert
        posts.Should().HaveCount(2);

        AssertExpectedPostData(cosmicPosts[0], posts[0]);
        AssertExpectedPostData(cosmicPosts[1], posts[1]);
    }

    private static void AssertExpectedPostData(CosmicPost expected, Post actual)
    {
        actual.Id.Should().Be(expected.Id);
        actual.Title.Should().Be(expected.Title);
        actual.Slug.Should().Be(expected.Slug);
        actual.DatePublished.Should().Be(expected.DatePublished);
        actual.Content.Should().Be(expected.Metadata.Markdown);
        actual.Description.Should().Be(expected.Metadata.Description);
        actual.Tags.Should().Be(expected.Metadata.Tags);
        actual.Image.Url.Should().Be(expected.Metadata.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Metadata.Image.ImgixUrl);
    }

    [Test]
    public void Map_CosmicPostToProtoPost_ReturnsProtoPost()
    {
        // arrange
        var readingTime = TimeSpan.FromMinutes(1);

        _mockReadTimeEstimator.Estimate(Arg.Any<string>()).Returns(readingTime);

        var cosmicPost = new CosmicPost(
            Id: "Id",
            Title: "Title",
            Slug: "Slug",
            DatePublished: DateTime.Now.ToUniversalTime(),
            Metadata: new CosmicPostMetadata(
                Description: "Description",
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
        AssertExpectedPostData(cosmicPost, protoPost);
    }

    [Test]
    public void Map_CosmicPostsToProtoPosts_ReturnsProtoPosts()
    {
        // arrange
        var readingTime = TimeSpan.FromMinutes(1);

        _mockReadTimeEstimator.Estimate(Arg.Any<string>()).Returns(readingTime);

        var cosmicPosts = new List<CosmicPost>
        {
            new(
                Id: "Id 1",
                Title: "Title 1",
                Slug: "Slug 1",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicPostMetadata(
                    Description: "Description 1",
                    Markdown: "Markdown 1",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 1",
                        ImgixUrl: "ImgixUrl 1"
                    )
                )
            ),
            new(
                Id: "Id 2",
                Title: "Title 2",
                Slug: "Slug 2",
                DatePublished: DateTime.Now.ToUniversalTime(),
                Metadata: new CosmicPostMetadata(
                    Description: "Description 2",
                    Markdown: "Markdown 2",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 2",
                        ImgixUrl: "ImgixUrl 2"
                    )
                )
            )
        };

        // act
        var protoPosts = _mapper.Map<CosmicPost, ProtoPost>(cosmicPosts).ToList();

        // assert
        protoPosts.Should().HaveCount(2);

        AssertExpectedPostData(cosmicPosts[0], protoPosts[0]);
        AssertExpectedPostData(cosmicPosts[1], protoPosts[1]);
    }

    private static void AssertExpectedPostData(CosmicPost expected, ProtoPost actual)
    {
        actual.Id.Should().Be(expected.Id);
        actual.Title.Should().Be(expected.Title);
        actual.Slug.Should().Be(expected.Slug);
        actual.DatePublished.Should().Be(expected.DatePublished.ToTimestamp());
        actual.Content.Should().Be(expected.Metadata.Markdown);
        actual.Description.Should().Be(expected.Metadata.Description);
        actual.Tags.Should().Be(expected.Metadata.Tags);
        actual.Image.Url.Should().Be(expected.Metadata.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Metadata.Image.ImgixUrl);
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
        AssertExpectedProjectData(cosmicProject, project);
    }

    [Test]
    public void Map_CosmicProjectsToProjects_ReturnsProjects()
    {
        // arrange
        var cosmicProjects = new List<CosmicProject>
        {
            new(
                Title: "Title 1",
                Metadata: new CosmicProjectMetadata(
                    Description: "Description 1",
                    Rank: 1,
                    GitHubUrl: "GitHubUrl 1",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 1",
                        ImgixUrl: "ImgixUrl 1"
                    )
                )
            ),
            new(
                Title: "Title 2",
                Metadata: new CosmicProjectMetadata(
                    Description: "Description 2",
                    Rank: 2,
                    GitHubUrl: "GitHubUrl 2",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 2",
                        ImgixUrl: "ImgixUrl 2"
                    )
                )
            )
        };

        // act
        var projects = _mapper.Map<CosmicProject, Project>(cosmicProjects).ToList();

        // assert
        projects.Should().HaveCount(2);

        AssertExpectedProjectData(cosmicProjects[0], projects[0]);
        AssertExpectedProjectData(cosmicProjects[1], projects[1]);
    }

    private static void AssertExpectedProjectData(CosmicProject expected, Project actual)
    {
        actual.Title.Should().Be(expected.Title);
        actual.Description.Should().Be(expected.Metadata.Description);
        actual.ProjectUrl.Should().Be(expected.Metadata.GitHubUrl);
        actual.Rank.Should().Be(expected.Metadata.Rank);
        actual.Tags.Should().Be(expected.Metadata.Tags);
        actual.Image.Url.Should().Be(expected.Metadata.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Metadata.Image.ImgixUrl);
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
        AssertExpectedProjectData(cosmicProject, protoProject);
    }

    [Test]
    public void Map_CosmicProjectsToProtoProjects_ReturnsProtoProjects()
    {
        // arrange
        var cosmicProjects = new List<CosmicProject>
        {
            new(
                Title: "Title 1",
                Metadata: new CosmicProjectMetadata(
                    Description: "Description 1",
                    Rank: 1,
                    GitHubUrl: "GitHubUrl 1",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 1",
                        ImgixUrl: "ImgixUrl 1"
                    )
                )
            ),
            new(
                Title: "Title 2",
                Metadata: new CosmicProjectMetadata(
                    Description: "Description 2",
                    Rank: 2,
                    GitHubUrl: "GitHubUrl 2",
                    Tags: "Tag1,Tag2",
                    Image: new CosmicImage(
                        Url: "Url 2",
                        ImgixUrl: "ImgixUrl 2"
                    )
                )
            )
        };

        // act
        var protoProjects = _mapper.Map<CosmicProject, ProtoProject>(cosmicProjects).ToList();

        // assert
        protoProjects.Should().HaveCount(2);

        AssertExpectedProjectData(cosmicProjects[0], protoProjects[0]);
        AssertExpectedProjectData(cosmicProjects[1], protoProjects[1]);
    }

    private static void AssertExpectedProjectData(CosmicProject expected, ProtoProject actual)
    {
        actual.Title.Should().Be(expected.Title);
        actual.Description.Should().Be(expected.Metadata.Description);
        actual.ProjectUrl.Should().Be(expected.Metadata.GitHubUrl);
        actual.Rank.Should().Be(expected.Metadata.Rank);
        actual.Tags.Should().Be(expected.Metadata.Tags);
        actual.Image.Url.Should().Be(expected.Metadata.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Metadata.Image.ImgixUrl);
    }
}
