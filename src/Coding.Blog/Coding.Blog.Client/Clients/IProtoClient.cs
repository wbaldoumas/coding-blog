namespace Coding.Blog.Client.Clients;

/// <summary>
///     An interface which adapts various Protobuf clients to a common API. This is needed
///     because Protobuf clients aren't generic and can't be forced to implement a common interface.
/// </summary>
/// <typeparam name="TProtoObject"></typeparam>
internal interface IProtoClient<TProtoObject>
{
    Task<IEnumerable<TProtoObject>> GetAsync();
}
