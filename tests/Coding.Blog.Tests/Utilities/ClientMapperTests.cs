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
public sealed class ClientMapperTests
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
        book.Title.Should().Be(protoBook.Title);
        book.Content.Should().Be(protoBook.Content);
        book.Author.Should().Be(protoBook.Author);
        book.DatePublished.Should().Be(protoBook.DatePublished.ToDateTime());
        book.PurchaseUrl.Should().Be(protoBook.PurchaseUrl);
        book.Image.Url.Should().Be(protoBook.Image.Url);
        book.Image.ImgixUrl.Should().Be(protoBook.Image.ImgixUrl);
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
        post.Id.Should().Be(protoPost.Id);
        post.Title.Should().Be(protoPost.Title);
        post.Slug.Should().Be(protoPost.Slug);
        post.ReadingTime.Should().Be(protoPost.ReadingTime.ToTimeSpan());
        post.DatePublished.Should().Be(protoPost.DatePublished.ToDateTime());
        post.Content.Should().Be(protoPost.Content);
        post.Description.Should().Be(protoPost.Description);
        post.Tags.Should().Be(protoPost.Tags);
        post.Image.Url.Should().Be(protoPost.Image.Url);
        post.Image.ImgixUrl.Should().Be(protoPost.Image.ImgixUrl);
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
