using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Services;

public interface IProjectsService
{
    Task<IEnumerable<Project>> GetAsync();
}
