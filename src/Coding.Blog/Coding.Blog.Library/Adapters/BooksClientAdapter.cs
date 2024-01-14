using Coding.Blog.Library.Protos;

namespace Coding.Blog.Library.Adapters;

public sealed class BooksClientAdapter(Books.BooksClient booksClient) : IProtoClientAdapter<Book>
{
    public async Task<IEnumerable<Book>> GetAsync()
    {
        var booksReply = await booksClient.GetBooksAsync(new BooksRequest()).ConfigureAwait(false);

        return booksReply.Books;
    }
}
