﻿using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Records;
using Grpc.Core;

namespace Coding.Blog.Library.Services;

public sealed class ProtoProjectsService(
    ICosmicClient<CosmicProjects> projectsClient,
    IMapper<CosmicProject, Project> projectMapper
) : Projects.ProjectsBase
{
    public override async Task<ProjectsReply> GetProjects(ProjectsRequest request, ServerCallContext context)
    {
        var cosmicProjects = await projectsClient.GetAsync().ConfigureAwait(false);
        var projects = projectMapper.Map(cosmicProjects.Projects);

        return new ProjectsReply
        {
            Projects = { projects }
        };
    }
}