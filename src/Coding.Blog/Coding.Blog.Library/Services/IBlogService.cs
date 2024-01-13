namespace Coding.Blog.Library.Services;

public interface IBlogService<T>
{
    Task<T> GetAsync();
}
