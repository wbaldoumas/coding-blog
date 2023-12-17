using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.Mappers;

public interface IPostLinker
{
    IEnumerable<Post> Link(IEnumerable<Post> posts);
}
