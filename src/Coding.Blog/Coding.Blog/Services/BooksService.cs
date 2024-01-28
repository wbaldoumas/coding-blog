using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Grpc.Core;

namespace Coding.Blog.Services;

internal sealed class BooksService(ICosmicClient<CosmicBook> client, IMapper mapper) : Books.BooksBase
{
    public override async Task<BooksReply> GetBooks(BooksRequest request, ServerCallContext context)
    {
        var cosmicBooks = await client.GetAsync().ConfigureAwait(false);
        var books = mapper.Map<CosmicBook, Book>(cosmicBooks);

        return new BooksReply
        {
            Books = { books }
        };
    }
}
