using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Utilities;
using Coding.Blog.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class ServerBlogServiceTests
{
    private ICosmicClient<CosmicBook> _mockClient = null!;

    private IMapper _mockMapper = null!;

    private BlogService<CosmicBook, Book> _blogService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = Substitute.For<ICosmicClient<CosmicBook>>();
        _mockMapper = Substitute.For<IMapper>();
        _blogService = new BlogService<CosmicBook, Book>(_mockClient, _mockMapper);
    }

    [Test]
    public async Task WhenBlogServiceIsInvoked_ThenBooksAreReturned()
    {
        // arrange
        _mockClient.GetAsync().Returns(ExpectedCosmicBooks);
        _mockMapper.Map<CosmicBook, Book>(ExpectedCosmicBooks).Returns(ExpectedBooks);

        // act
        var books = await _blogService.GetAsync().ConfigureAwait(false);

        // assert
        books.Should().BeEquivalentTo(ExpectedBooks);
    }

    private static readonly CosmicBook ExpectedCosmicBook = new(
        Title: "some book",
        DatePublished: DateTime.UtcNow,
        Metadata: new CosmicBookMetadata(
            PurchaseUrl: "some url",
            Image: new CosmicImage(
                Url: "some image url",
                ImgixUrl: "some imgix url"
            ),
            Content: "some content",
            Author: "some author"
        )
    );

    private static readonly List<CosmicBook> ExpectedCosmicBooks = [ExpectedCosmicBook];

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
}
