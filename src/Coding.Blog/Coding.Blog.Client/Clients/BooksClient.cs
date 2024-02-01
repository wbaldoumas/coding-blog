using Coding.Blog.Library.Protos;

namespace Coding.Blog.Client.Clients;

/// <summary>
///     Wraps the <see cref="Books.BooksClient"/> to provide a more idiomatic, generic interface.
/// </summary>
/// <param name="booksClient">The <see cref="Books.BooksClient"/> to wrap.</param>
internal sealed class BooksClient(Books.BooksClient booksClient) : IProtoClient<Book>
{
    public async Task<IEnumerable<Book>> GetAsync()
    {
        var booksReply = await booksClient.GetBooksAsync(new BooksRequest()).ConfigureAwait(false);

        return booksReply.Books;
    }
}
