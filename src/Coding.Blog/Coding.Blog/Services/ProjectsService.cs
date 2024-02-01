using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Grpc.Core;

namespace Coding.Blog.Services;

/// <summary>
///     A gRPC service for the projects endpoint.
/// </summary>
/// <param name="client">The <see cref="ICosmicClient{T}"/> to retrieve projects from.</param>
/// <param name="mapper">A <see cref="IMapper"/> for mapping objects to their protobuf representations.</param>
internal sealed class ProjectsService(ICosmicClient<CosmicProject> client, IMapper mapper) : Projects.ProjectsBase
{
    public override async Task<ProjectsReply> GetProjects(ProjectsRequest request, ServerCallContext context)
    {
        var cosmicProjects = await client.GetAsync().ConfigureAwait(false);
        var projects = mapper.Map<CosmicProject, Project>(cosmicProjects);

        return new ProjectsReply
        {
            Projects = { projects }
        };
    }
}
