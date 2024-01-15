using Coding.Blog.Library.Protos;

namespace Coding.Blog.Library.Clients;

public sealed class BooksClient(Books.BooksClient booksClient) : IProtoClient<Book>
{
    public async Task<IEnumerable<Book>> GetAsync()
    {
        var booksReply = await booksClient.GetBooksAsync(new BooksRequest()).ConfigureAwait(false);

        return booksReply.Books;
    }
}
