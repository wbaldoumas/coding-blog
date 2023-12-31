using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Utilities;

public interface IPostLinker
{
    IEnumerable<Post> Link(IEnumerable<Post> posts);
}
