using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Mappers;

namespace Coding.Blog.Library.Services;

public sealed class BlogService<TCosmicObject, TDomainObject>(
    ICosmicClient<TCosmicObject> client,
    IMapper<TCosmicObject, TDomainObject> mapper
) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var cosmicObjects = await client.GetAsync().ConfigureAwait(false);

        return mapper.Map(cosmicObjects);
    }
}
