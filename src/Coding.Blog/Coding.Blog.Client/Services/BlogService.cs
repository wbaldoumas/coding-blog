using Coding.Blog.Client.Clients;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Client.Services;

internal sealed class BlogService<TProtoObject, TDomainObject>(IProtoClient<TProtoObject> client, IMapper mapper) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var protoObjects = await client.GetAsync().ConfigureAwait(false);

        return mapper.Map<TProtoObject, TDomainObject>(protoObjects);
    }
}
