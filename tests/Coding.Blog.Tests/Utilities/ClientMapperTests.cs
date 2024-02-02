using Coding.Blog.Client.Utilities;
using Coding.Blog.Library.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;


namespace Coding.Blog.Tests.Utilities;

[TestFixture]
internal sealed class ClientMapperTests
{
    private Mapper _mapper = null!;

    [SetUp]
    public void SetUp() => _mapper = new Mapper();

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
        AssertExpectedBookData(protoBook, book);
    }

    [Test]
    public void Map_ProtoBooksToBooks_ReturnsBooks()
    {
        // arrange
        var protoBooks = new List<ProtoBook>
        {
            new()
            {
                Title = "Title 1",
                Content = "Content 1",
                Author = "Author 1",
                DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
                PurchaseUrl = "PurchaseUrl 1",
                Image = new Image
                {
                    Url = "Url 1",
                    ImgixUrl = "ImgixUrl 1"
                }
            },
            new()
            {
                Title = "Title 2",
                Author = "Author 2",
                Content = "Content 2",
                DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
                PurchaseUrl = "PurchaseUrl 2",
                Image = new Image
                {
                    Url = "Url 2",
                    ImgixUrl = "ImgixUrl 2"
                }
            }
        };

        // act
        var books = _mapper.Map<ProtoBook, Book>(protoBooks).ToList();

        // assert
        books.Should().HaveCount(2);

        AssertExpectedBookData(protoBooks[0], books[0]);
        AssertExpectedBookData(protoBooks[1], books[1]);
    }

    private static void AssertExpectedBookData(ProtoBook expected, Book actual)
    {
        actual.Title.Should().Be(expected.Title);
        actual.Content.Should().Be(expected.Content);
        actual.Author.Should().Be(expected.Author);
        actual.DatePublished.Should().Be(expected.DatePublished.ToDateTime());
        actual.PurchaseUrl.Should().Be(expected.PurchaseUrl);
        actual.Image.Url.Should().Be(expected.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Image.ImgixUrl);
    }

    [Test]
    public void Map_ProtoPostToPost_ReturnsPost()
    {
        // arrange
        var protoPost = new ProtoPost
        {
            Id = "Id",
            Title = "Title",
            Slug = "Slug",
            ReadingTime = TimeSpan.FromMinutes(1).ToDuration(),
            DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
            Content = "Content",
            Description = "Description",
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
        AssertExpectedPostData(protoPost, post);
    }

    [Test]
    public void Map_ProtoPostsToPosts_ReturnsPosts()
    {
        // arrange
        var protoPosts = new List<ProtoPost>
        {
            new()
            {
                Id = "Id 1",
                Title = "Title 1",
                Slug = "Slug 1",
                ReadingTime = TimeSpan.FromMinutes(1).ToDuration(),
                DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
                Content = "Content 1",
                Description = "Description 1",
                Tags = "Tag1,Tag2",
                Image = new Image
                {
                    Url = "Url 1",
                    ImgixUrl = "ImgixUrl 1"
                }
            },
            new()
            {
                Id = "Id 2",
                Title = "Title 2",
                Slug = "Slug 2",
                ReadingTime = TimeSpan.FromMinutes(2).ToDuration(),
                DatePublished = DateTime.Now.ToUniversalTime().ToTimestamp(),
                Content = "Content 2",
                Description = "Description 2",
                Tags = "Tag1,Tag2",
                Image = new Image
                {
                    Url = "Url 2",
                    ImgixUrl = "ImgixUrl 2"
                }
            }
        };

        // act
        var posts = _mapper.Map<ProtoPost, Post>(protoPosts).ToList();

        // assert
        posts.Should().HaveCount(2);

        AssertExpectedPostData(protoPosts[0], posts[0]);
        AssertExpectedPostData(protoPosts[1], posts[1]);
    }

    private static void AssertExpectedPostData(ProtoPost expected, Post actual)
    {
        actual.Id.Should().Be(expected.Id);
        actual.Title.Should().Be(expected.Title);
        actual.Slug.Should().Be(expected.Slug);
        actual.ReadingTime.Should().Be(expected.ReadingTime.ToTimeSpan());
        actual.DatePublished.Should().Be(expected.DatePublished.ToDateTime());
        actual.Content.Should().Be(expected.Content);
        actual.Description.Should().Be(expected.Description);
        actual.Tags.Should().Be(expected.Tags);
        actual.Image.Url.Should().Be(expected.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Image.ImgixUrl);
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
        AssertExpectedProjectData(protoProject, project);
    }

    [Test]
    public void Map_ProtoProjectsToProjects_ReturnsProjects()
    {
        // arrange
        var protoProjects = new List<ProtoProject>
        {
            new()
            {
                Title = "Title 1",
                Description = "Description 1",
                ProjectUrl = "ProjectUrl 1",
                Rank = 1,
                Tags = "Tag1,Tag2",
                Image = new Image
                {
                    Url = "Url 1",
                    ImgixUrl = "ImgixUrl 1"
                }
            },
            new()
            {
                Title = "Title 2",
                Description = "Description 2",
                ProjectUrl = "ProjectUrl 2",
                Rank = 2,
                Tags = "Tag1,Tag2",
                Image = new Image
                {
                    Url = "Url 2",
                    ImgixUrl = "ImgixUrl 2"
                }
            }
        };

        // act
        var projects = _mapper.Map<ProtoProject, Project>(protoProjects).ToList();

        // assert
        projects.Should().HaveCount(2);

        AssertExpectedProjectData(protoProjects[0], projects[0]);
        AssertExpectedProjectData(protoProjects[1], projects[1]);
    }

    private static void AssertExpectedProjectData(ProtoProject expected, Project actual)
    {
        actual.Title.Should().Be(expected.Title);
        actual.Description.Should().Be(expected.Description);
        actual.ProjectUrl.Should().Be(expected.ProjectUrl);
        actual.Rank.Should().Be(expected.Rank);
        actual.Tags.Should().Be(expected.Tags);
        actual.Image.Url.Should().Be(expected.Image.Url);
        actual.Image.ImgixUrl.Should().Be(expected.Image.ImgixUrl);
    }
}
