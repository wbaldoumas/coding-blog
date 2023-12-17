using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.Services;

public interface IPostsService
{
    Task<IEnumerable<Post>> GetAsync();
}
