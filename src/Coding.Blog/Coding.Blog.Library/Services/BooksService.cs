using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Records;
using Grpc.Core;

namespace Coding.Blog.Library.Services;

public sealed class BooksService(
    ICosmicClient<CosmicBook> booksClient,
    IMapper<CosmicBook, Book> bookMapper
) : Books.BooksBase
{
    public override async Task<BooksReply> GetBooks(BooksRequest request, ServerCallContext context)
    {
        var cosmicBooks = await booksClient.GetAsync().ConfigureAwait(false);
        var books = bookMapper.Map(cosmicBooks);

        return new BooksReply
        {
            Books = { books }
        };
    }
}
