using Coding.Blog.Clients;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Services;

/// <summary>
///     A generic service for retrieving blog posts from the Cosmic API and mapping them to domain objects.
/// </summary>
/// <typeparam name="TCosmicObject"> The type of object to retrieve from the Cosmic API. </typeparam>
/// <typeparam name="TDomainObject"> The type of object to map to. </typeparam>
/// <param name="client"> The client to retrieve the objects from. </param>
/// <param name="mapper"> The mapper to map the objects to domain objects. </param>
internal sealed class BlogService<TCosmicObject, TDomainObject>(ICosmicClient<TCosmicObject> client, IMapper mapper) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var cosmicObjects = await client.GetAsync().ConfigureAwait(false);

        return mapper.Map<TCosmicObject, TDomainObject>(cosmicObjects);
    }
}
