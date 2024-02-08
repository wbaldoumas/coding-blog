using Coding.Blog.Client.Clients;
using Coding.Blog.Library.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Clients;

[TestFixture]
internal sealed class BooksClientTests
{
    private Books.BooksClient _mockProtoBooksClient = null!;

    private BooksClient _booksClient = null!;

    [SetUp]
    public void SetUp()
    {
        _mockProtoBooksClient = Substitute.For<Books.BooksClient>();
        _booksClient = new BooksClient(_mockProtoBooksClient);
    }

    [Test]
    public async Task WhenClientIsInvoked_ThenBooksAreReturned()
    {
        // arrange
        using var booksResponse = new AsyncUnaryCall<BooksReply>(
            Task.FromResult(new BooksReply
                {
                    Books = { ExpectedBooks }
                }
            ),
            Task.FromResult(Metadata.Empty),
            () => Status.DefaultSuccess,
            () => Metadata.Empty,
            () => { }
        );

        _mockProtoBooksClient.GetBooksAsync(Arg.Any<BooksRequest>()).Returns(booksResponse);

        // act
        var actualBooks = await _booksClient.GetAsync().ConfigureAwait(false);

        // assert
        actualBooks.Should().BeEquivalentTo(ExpectedBooks);
    }

    private static readonly Book ExpectedBook = new()
    {
        Title = "some book",
        Author = "some author",
        Content = "some content",
        DatePublished = DateTime.UtcNow.ToTimestamp(),
        Image = new Image
        {
            Url = "some url",
            ImgixUrl = "some other url"
        },
        PurchaseUrl = "some other other url"
    };

    private static readonly List<Book> ExpectedBooks = [ExpectedBook];
}
