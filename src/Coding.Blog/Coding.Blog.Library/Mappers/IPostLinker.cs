using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Mappers;

public interface IPostLinker
{
    IEnumerable<Post> Link(IEnumerable<Post> posts);
}
