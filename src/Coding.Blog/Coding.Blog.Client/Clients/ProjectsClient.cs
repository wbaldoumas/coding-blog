using Coding.Blog.Library.Protos;

namespace Coding.Blog.Client.Clients;

/// <summary>
///     Wraps the <see cref="Projects.ProjectsClient"/> to provide a more idiomatic, generic interface.
/// </summary>
/// <param name="projectsClient">The <see cref="Projects.ProjectsClient"/> to wrap.</param>
internal sealed class ProjectsClient(Projects.ProjectsClient projectsClient) : IProtoClient<Project>
{
    public async Task<IEnumerable<Project>> GetAsync()
    {
        var projectsReply = await projectsClient.GetProjectsAsync(new ProjectsRequest()).ConfigureAwait(false);

        return projectsReply.Projects;
    }
}
