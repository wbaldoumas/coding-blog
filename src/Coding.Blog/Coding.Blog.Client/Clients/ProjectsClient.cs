﻿using Coding.Blog.Library.Protos;

namespace Coding.Blog.Client.Clients;

internal sealed class ProjectsClient(Projects.ProjectsClient projectsClient) : IProtoClient<Project>
{
    public async Task<IEnumerable<Project>> GetAsync()
    {
        var projectsReply = await projectsClient.GetProjectsAsync(new ProjectsRequest()).ConfigureAwait(false);

        return projectsReply.Projects;
    }
}