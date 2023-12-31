using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Services;

public interface IBooksService
{
    Task<IEnumerable<Book>> GetAsync();
}
