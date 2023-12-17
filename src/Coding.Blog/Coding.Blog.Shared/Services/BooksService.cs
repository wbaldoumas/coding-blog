using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Services;

internal sealed class BooksService(
    ICosmicClient<CosmicBooks> booksClient,
    IMapper<CosmicBook, Book> booksMapper
) : IBooksService
{
    public async Task<IEnumerable<Book>> GetAsync()
    {
        var cosmicBooks = await booksClient.GetAsync().ConfigureAwait(false);

        return booksMapper.Map(cosmicBooks.Books);
    }
}
