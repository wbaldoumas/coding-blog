using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.State;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Text.Json;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class PersistentComponentStateServiceTests
{
    private ComponentStatePersistenceManager _mockComponentStatePersistenceManager = null!;

    private IBlogService<Book> _mockBlogService = null!;

    private PersistentComponentStateService<Book> _persistentComponentStateService = null!;

    [SetUp]
    public void SetUp()
    {
        BooksState.Books = new List<Book>();

        _mockComponentStatePersistenceManager = new ComponentStatePersistenceManager(
            Substitute.For<ILogger<ComponentStatePersistenceManager>>()
        );

        _mockBlogService = Substitute.For<IBlogService<Book>>();

        _persistentComponentStateService = new PersistentComponentStateService<Book>(_mockBlogService);
    }

    [Test]
    public async Task WhenBooksAreNotInComponentStateOrStaticState_ThenBlogServiceIsInvoked()
    {
        // arrange
        _mockBlogService.GetAsync().Returns(ExpectedBooks);

        // act
        var actualBooks = await _persistentComponentStateService.GetAsync(
            _mockComponentStatePersistenceManager.State,
            Book.Key
        ).ConfigureAwait(false);

        // assert
        actualBooks.Should().BeEquivalentTo(ExpectedBooks);

        await _mockBlogService.Received(1).GetAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task WhenBooksAreInComponentState_ThenBlogServiceIsNotInvoked()
    {
        // arrange
        var mockPersistentComponentStateStore = Substitute.For<IPersistentComponentStateStore>();

        mockPersistentComponentStateStore.GetPersistedStateAsync().Returns(
            new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase)
            {
                { Book.Key, JsonSerializer.SerializeToUtf8Bytes<IList<Book>>(ExpectedBooks) }
            });

        await _mockComponentStatePersistenceManager.RestoreStateAsync(mockPersistentComponentStateStore).ConfigureAwait(false);

        // act
        var actualBooks = await _persistentComponentStateService.GetAsync(_mockComponentStatePersistenceManager.State, Book.Key).ConfigureAwait(false);

        // assert
        actualBooks.Should().BeEquivalentTo(ExpectedBooks);

        await _mockBlogService.DidNotReceiveWithAnyArgs().GetAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task WhenBooksAreNotInComponentStateButInStaticState_ThenBlogServiceIsNotInvoked()
    {
        // arrange
        BooksState.Books = ExpectedBooks;

        // act
        var actualBooks = await _persistentComponentStateService.GetAsync(_mockComponentStatePersistenceManager.State, Book.Key).ConfigureAwait(false);

        // assert
        actualBooks.Should().BeEquivalentTo(ExpectedBooks);

        await _mockBlogService.DidNotReceiveWithAnyArgs().GetAsync().ConfigureAwait(false);
    }

    private static readonly Book ExpectedBook = new(
        Title: "some title",
        Content: "some content",
        Author: "some author",
        Image: new Image("some url"),
        Url: "some purchase url",
        DatePublished: DateTime.Now
    );

    private static readonly List<Book> ExpectedBooks = [ExpectedBook];
}
