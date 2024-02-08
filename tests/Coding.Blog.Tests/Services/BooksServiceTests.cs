using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Coding.Blog.Services;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class BooksServiceTests
{
    private ICosmicClient<CosmicBook> _mockClient = null!;

    private IMapper _mockMapper = null!;

    private BooksService _booksService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = Substitute.For<ICosmicClient<CosmicBook>>();
        _mockMapper = Substitute.For<IMapper>();
        _booksService = new BooksService(_mockClient, _mockMapper);
    }

    [Test]
    public async Task WhenBooksServiceIsInvoked_ThenBooksAreReturned()
    {
        // arrange
        _mockClient
            .GetAsync()
            .Returns(ExpectedCosmicBooks);

        _mockMapper
            .Map<CosmicBook, Book>(ExpectedCosmicBooks)
            .Returns(ExpectedBooks);

        var request = new BooksRequest();
        var serverCallContext = Substitute.For<ServerCallContext>();

        // act
        var actualBooks = await _booksService.GetBooks(request, serverCallContext);

        // assert
        actualBooks.Books.Should().BeEquivalentTo(ExpectedBooks);
    }

    private static readonly CosmicBook ExpectedCosmicBook = new(
        Title: "some title",
        DatePublished: DateTime.UtcNow,
        Metadata: new CosmicBookMetadata(
            Author: "some author",
            Image: new CosmicImage(
                Url: "some url",
                ImgixUrl: "some other url"
            ),
            PurchaseUrl: "some url",
            Content: "some content"
        )
    );

    private static readonly List<CosmicBook> ExpectedCosmicBooks = [ExpectedCosmicBook];

    private static readonly Book ExpectedBook = new()
    {
        Title = "some title",
        Content = "some content",
        Image = new Image
        {
            Url = "some url",
            ImgixUrl = "some other url"
        },
        Author = "some author",
        DatePublished = DateTime.UtcNow.ToTimestamp(),
        PurchaseUrl = "some url"
    };

    private static readonly List<Book> ExpectedBooks = [ExpectedBook];
}
