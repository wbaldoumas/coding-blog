using Coding.Blog.Library.Adapters;
using Coding.Blog.Library.Mappers;

namespace Coding.Blog.Library.Services;

public sealed class ClientBlogService<TProtoObject, TDomainObject>(
    IProtoClientAdapter<TProtoObject> protoClient,
    IMapper<TProtoObject, TDomainObject> mapper
) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var protoObjects = await protoClient.GetAsync().ConfigureAwait(false);

        return mapper.Map(protoObjects);
    }
}
