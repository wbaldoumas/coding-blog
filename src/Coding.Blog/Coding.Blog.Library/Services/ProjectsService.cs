using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Records;

namespace Coding.Blog.Library.Services;

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
