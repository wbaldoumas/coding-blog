using Coding.Blog.Library.Domain;
using Coding.Blog.Library.State;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public sealed class PersistentProjectsService(IBlogService<IEnumerable<Project>> projectsService) : IPersistentService<IList<Project>>
{
    public async Task<IList<Project>> GetAsync(PersistentComponentState applicationState, string stateKey)
    {
        if (applicationState.TryTakeFromJson<IList<Project>>(stateKey, out var projectsState))
        {
            ProjectsState.Projects = projectsState ?? new List<Project>();
        }

        if (ProjectsState.Projects.Any())
        {
            return ProjectsState.Projects;
        }

        ProjectsState.Projects = await GetProjectsAsync().ConfigureAwait(false);

        return ProjectsState.Projects;
    }

    private async Task<IList<Project>> GetProjectsAsync()
    {
        var projects = await projectsService.GetAsync().ConfigureAwait(false);

        return projects.OrderBy(project => project.Rank).ToList();
    }
}
