using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Protos;
using Book = Coding.Blog.Library.Domain.Book;
using ProtoBook = Coding.Blog.Library.Protos.Book;

namespace Coding.Blog.Library.Services;

public sealed class ClientBooksService(
    Books.BooksClient booksClient,
    IMapper<ProtoBook, Book> bookMapper
) : IBooksService
{
    public async Task<IEnumerable<Book>> GetAsync()
    {
        var booksReply = await booksClient.GetBooksAsync(new BooksRequest()).ConfigureAwait(false);

        return bookMapper.Map(booksReply.Books);
    }
}
