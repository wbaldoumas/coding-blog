using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Library.Services;

public sealed class BlogService<TCosmicObject, TDomainObject>(ICosmicClient<TCosmicObject> client, IMapper mapper) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var cosmicObjects = await client.GetAsync().ConfigureAwait(false);

        return mapper.Map<TCosmicObject, TDomainObject>(cosmicObjects);
    }
}
