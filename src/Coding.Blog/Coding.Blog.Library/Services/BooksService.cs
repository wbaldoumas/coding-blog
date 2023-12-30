using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Records;

namespace Coding.Blog.Library.Services;

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
