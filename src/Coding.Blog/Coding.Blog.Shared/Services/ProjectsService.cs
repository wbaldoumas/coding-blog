using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Services;

public sealed class ProjectsService(
    ICosmicClient<CosmicProjects> projectsClient,
    IMapper<CosmicProject, Project> projectsMapper
) : IProjectsService
{
    public async Task<IEnumerable<Project>> GetAsync()
    {
        var cosmicProjects = await projectsClient.GetAsync().ConfigureAwait(false);
        var projects = projectsMapper.Map(cosmicProjects.Projects);

        return projects.OrderBy(project => project.Rank);
    }
}
