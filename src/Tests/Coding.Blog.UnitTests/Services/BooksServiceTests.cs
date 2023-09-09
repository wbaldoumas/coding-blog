using Coding.Blog.Engine;
using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Services;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;
using Grpc.Core.Testing;

namespace Coding.Blog.UnitTests.Services;

[TestFixture]
internal sealed class BooksServiceTests
{
    private ICosmicClient<CosmicBooks> _mockCosmicBookClient = default!;
    private IMapper<CosmicBook, Book> _mockBookMapper = default!;

    [SetUp]
    public void SetUp()
    {
        _mockCosmicBookClient = Substitute.For<ICosmicClient<CosmicBooks>>();
        _mockBookMapper = Substitute.For<IMapper<CosmicBook, Book>>();
    }

    [Test]
    public async Task BooksService_generates_expected_response_when_books_exist()
    {
        // arrange
        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var mockCosmicBooksClientResponse = new CosmicBooks(
            new List<CosmicBook>
            {
                new("BookA", "ContentA", mockDatePublished, new CosmicBookMetadata("UrlA", new CosmicBookCoverMetadata("CoverA"))),
                new("BookB", "ContentB", mockDatePublished, new CosmicBookMetadata("UrlB", new CosmicBookCoverMetadata("CoverB"))),
                new("BookC", "ContentC", mockDatePublished, new CosmicBookMetadata("UrlC", new CosmicBookCoverMetadata("CoverC")))
            }
        );

        _mockCosmicBookClient
            .GetAsync()
            .Returns(mockCosmicBooksClientResponse);

        var mockBookMapperResponse = mockCosmicBooksClientResponse.Books.Select(cosmicBook => new Book
        {
            Title = cosmicBook.Title,
            Content = cosmicBook.Content,
            PurchaseUrl = cosmicBook.Metadata.PurchaseUrl,
            CoverImageUrl = cosmicBook.Metadata.Cover.Url,
            DatePublished = Timestamp.FromDateTime(mockDatePublished)
        });

        _mockBookMapper
            .Map(mockCosmicBooksClientResponse.Books)
            .Returns(mockBookMapperResponse);

        var mockServerCallContext = TestServerCallContext.Create(
            "GetBooks",
            "TestHost",
            DateTime.MaxValue,
            Metadata.Empty,
            CancellationToken.None,
            "TestPeer",
            new AuthContext("TestPeerIdentity", new Dictionary<string, List<AuthProperty>>(StringComparer.OrdinalIgnoreCase)),
            null,
            _ => Task.CompletedTask,
            () => new WriteOptions(),
            _ => { }
        );

        var service = new BooksService(_mockCosmicBookClient, _mockBookMapper);

        // act
        var response = await service.GetBooks(new BooksRequest(), mockServerCallContext);

        // assert
        response.Should().NotBeNull();
        response.Books.Should().HaveCount(3);

        await _mockCosmicBookClient
            .Received(1)
            .GetAsync();

        _mockBookMapper
            .Received(1)
            .Map(mockCosmicBooksClientResponse.Books);
    }

    [Test]
    public async Task BooksService_generates_expected_response_when_books_do_not_exist()
    {
        // arrange
        var mockCosmicBooksClientResponse = new CosmicBooks(new List<CosmicBook>());

        _mockCosmicBookClient
            .GetAsync()
            .Returns(mockCosmicBooksClientResponse);

        _mockBookMapper
            .Map(mockCosmicBooksClientResponse.Books)
            .Returns(new List<Book>());

        var mockServerCallContext = TestServerCallContext.Create(
            "GetBooks",
            "TestHost",
            DateTime.MaxValue,
            Metadata.Empty,
            CancellationToken.None,
            "TestPeer",
            new AuthContext("TestPeerIdentity", new Dictionary<string, List<AuthProperty>>(StringComparer.OrdinalIgnoreCase)),
            null,
            _ => Task.CompletedTask,
            () => new WriteOptions(),
            _ => { }
        );

        var service = new BooksService(_mockCosmicBookClient, _mockBookMapper);

        // act
        var response = await service.GetBooks(new BooksRequest(), mockServerCallContext);

        // assert
        response.Should().NotBeNull();
        response.Books.Should().BeEmpty();

        await _mockCosmicBookClient
            .Received(1)
            .GetAsync();

        _mockBookMapper
            .Received(1)
            .Map(mockCosmicBooksClientResponse.Books);
    }
}