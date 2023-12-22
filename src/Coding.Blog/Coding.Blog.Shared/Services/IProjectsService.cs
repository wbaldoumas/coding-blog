using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.Services;

public interface IProjectsService
{
    Task<IEnumerable<Project>> GetAsync();
}
