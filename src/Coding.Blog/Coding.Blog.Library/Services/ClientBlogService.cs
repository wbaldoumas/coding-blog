using Coding.Blog.Library.Adapters;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Library.Services;

public sealed class ClientBlogService<TProtoObject, TDomainObject>(IProtoClientAdapter<TProtoObject> client, IMapper mapper) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var protoObjects = await client.GetAsync().ConfigureAwait(false);

        return mapper.Map<TProtoObject, TDomainObject>(protoObjects);
    }
}
