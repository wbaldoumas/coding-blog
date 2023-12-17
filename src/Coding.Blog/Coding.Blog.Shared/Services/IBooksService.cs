using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.Services;

public interface IBooksService
{
    Task<IEnumerable<Book>> GetAsync();
}
