using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Protos;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Library.Services;

public sealed class ClientProjectsService(
    Projects.ProjectsClient projectsClient,
    IMapper<ProtoProject, Project> projectMapper
) : IProjectsService
{
    public async Task<IEnumerable<Project>> GetAsync()
    {
        var projectsReply = await projectsClient.GetProjectsAsync(new ProjectsRequest()).ConfigureAwait(false);

        return projectMapper.Map(projectsReply.Projects);
    }
}
