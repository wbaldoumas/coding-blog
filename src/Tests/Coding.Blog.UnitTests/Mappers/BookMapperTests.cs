using Coding.Blog.Engine;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Mappers;

[TestFixture]
internal sealed class BookMapperTests
{
    [Test]
    public void Map_generates_expected_book()
    {
        // arrange
        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var cosmicBook = new CosmicBook(
            "Test Book",
            "This is some content talking about Test Book",
            mockDatePublished,
            new CosmicBookMetadata("https://www.amazon.com/",
                new CosmicBookCoverMetadata("https://www.amazon.com/")
            )
        );

        var expectedBook = new Book
        {
            Title = "Test Book",
            Content = "This is some content talking about Test Book",
            PurchaseUrl = "https://www.amazon.com/",
            CoverImageUrl = "https://www.amazon.com/",
            DatePublished = Timestamp.FromDateTime(mockDatePublished)
        };

        var mapper = new BookMapper();

        // act
        var book = mapper.Map(cosmicBook);

        // assert
        book.Should().BeEquivalentTo(expectedBook);
    }

    [Test]
    public void Map_generates_expected_books()
    {
        // arrange
        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var cosmicBookA = new CosmicBook(
            "Test Book A",
            "This is some content talking about Test Book A",
            mockDatePublished,
            new CosmicBookMetadata("https://www.amazon.com/a",
                new CosmicBookCoverMetadata("https://www.amazon.com/a")
            )
        );

        var cosmicBookB = new CosmicBook(
            "Test Book B",
            "This is some content talking about Test Book B",
            mockDatePublished,
            new CosmicBookMetadata("https://www.amazon.com/b",
                new CosmicBookCoverMetadata("https://www.amazon.com/b")
            )
        );

        var cosmicBooks = new List<CosmicBook>
        {
            cosmicBookA,
            cosmicBookB
        };

        var expectedBookA = new Book
        {
            Title = "Test Book A",
            Content = "This is some content talking about Test Book A",
            PurchaseUrl = "https://www.amazon.com/a",
            CoverImageUrl = "https://www.amazon.com/a",
            DatePublished = Timestamp.FromDateTime(mockDatePublished)
        };

        var expectedBookB = new Book
        {
            Title = "Test Book B",
            Content = "This is some content talking about Test Book B",
            PurchaseUrl = "https://www.amazon.com/b",
            CoverImageUrl = "https://www.amazon.com/b",
            DatePublished = Timestamp.FromDateTime(mockDatePublished)
        };

        var expectedBooks = new List<Book>
        {
            expectedBookA,
            expectedBookB
        };

        var mapper = new BookMapper();

        // act
        var books = mapper.Map(cosmicBooks);

        // assert
        books.Should().BeEquivalentTo(expectedBooks);
    }
}