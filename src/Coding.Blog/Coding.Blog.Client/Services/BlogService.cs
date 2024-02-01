using Coding.Blog.Client.Clients;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Client.Services;

/// <summary>
///     A client-side service for retrieving blog data.
/// </summary>
/// <typeparam name="TProtoObject">The type of object to retrieve from the gRPC API.</typeparam>
/// <typeparam name="TDomainObject">The type of the domain object to return.</typeparam>
/// <param name="client">A gRPC client for retrieving the <see cref="TProtoObject"/> from the gRPC API.</param>
/// <param name="mapper">A mapper for mapping the <see cref="TProtoObject"/> to the <see cref="TDomainObject"/>.</param>
internal sealed class BlogService<TProtoObject, TDomainObject>(IProtoClient<TProtoObject> client, IMapper mapper) : IBlogService<TDomainObject>
{
    public async Task<IEnumerable<TDomainObject>> GetAsync()
    {
        var protoObjects = await client.GetAsync().ConfigureAwait(false);

        return mapper.Map<TProtoObject, TDomainObject>(protoObjects);
    }
}
