namespace Coding.Blog.Library.Adapters;

/// <summary>
///     An interface which adapts various Protobuf clients to a common interface. This is needed
///     because Protobuf clients aren't generic and can't be forced to implement a common interface.
/// </summary>
/// <typeparam name="TProtoObject"></typeparam>
public interface IProtoClientAdapter<TProtoObject>
{
    Task<IEnumerable<TProtoObject>> GetAsync();
}
