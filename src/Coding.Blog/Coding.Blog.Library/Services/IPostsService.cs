using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Services;

public interface IPostsService
{
    Task<IEnumerable<Post>> GetAsync();
}
