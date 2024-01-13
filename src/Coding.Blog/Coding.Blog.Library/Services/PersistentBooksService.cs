using Coding.Blog.Library.Domain;
using Coding.Blog.Library.State;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public sealed class PersistentBooksService(IBlogService<IEnumerable<Book>> booksService) : IPersistentService<IList<Book>>
{
    public async Task<IList<Book>> GetAsync(PersistentComponentState applicationState, string stateKey)
    {
        if (applicationState.TryTakeFromJson<IList<Book>>(stateKey, out var booksState))
        {
            BooksState.Books = booksState ?? new List<Book>();
        }

        if (BooksState.Books.Any())
        {
            return BooksState.Books;
        }

        BooksState.Books = await GetBooksAsync().ConfigureAwait(false);

        return BooksState.Books;
    }

    private async Task<IList<Book>> GetBooksAsync()
    {
        var books = await booksService.GetAsync().ConfigureAwait(false);

        return books.ToList();
    }
}
