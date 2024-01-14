using Coding.Blog.Library.Protos;

namespace Coding.Blog.Library.Adapters;

public sealed class ProjectsClientAdapter(Projects.ProjectsClient projectsClient) : IProtoClientAdapter<Project>
{
    public async Task<IEnumerable<Project>> GetAsync()
    {
        var projectsReply = await projectsClient.GetProjectsAsync(new ProjectsRequest()).ConfigureAwait(false);

        return projectsReply.Projects;
    }
}
