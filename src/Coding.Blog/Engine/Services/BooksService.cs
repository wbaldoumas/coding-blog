using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Grpc.Core;

namespace Coding.Blog.Engine.Services;

public sealed class BooksService : Books.BooksBase
{
    private readonly ICosmicClient<CosmicBooks> _cosmicClient;
    private readonly IMapper<CosmicBook, Book> _mapper;

    public BooksService(ICosmicClient<CosmicBooks> cosmicClient, IMapper<CosmicBook, Book> mapper)
    {
        _cosmicClient = cosmicClient;
        _mapper = mapper;
    }

    public override async Task<BooksReply> GetBooks(BooksRequest request, ServerCallContext context)
    {
        var cosmicBooks = await _cosmicClient.GetAsync().ConfigureAwait(false);

        return new BooksReply
        { 
            Books = { _mapper.Map(cosmicBooks.Books) }
        };
    }
}
