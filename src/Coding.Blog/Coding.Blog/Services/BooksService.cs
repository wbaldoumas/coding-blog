using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Grpc.Core;

namespace Coding.Blog.Services;

/// <summary>
///     A gRPC service for the Books endpoint.
/// </summary>
/// <param name="client">The <see cref="ICosmicClient{T}"/> to retrieve books from.</param>
/// <param name="mapper">A <see cref="IMapper"/> for mapping objects to their protobuf representations.</param>
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
