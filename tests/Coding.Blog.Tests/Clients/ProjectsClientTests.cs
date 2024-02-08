using Coding.Blog.Client.Clients;
using Coding.Blog.Library.Protos;
using FluentAssertions;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Clients;

[TestFixture]
internal sealed class ProjectsClientTests
{
    private Projects.ProjectsClient _mockProtoProjectsClient = null!;

    private ProjectsClient _projectsClient = null!;

    [SetUp]
    public void SetUp()
    {
        _mockProtoProjectsClient = Substitute.For<Projects.ProjectsClient>();
        _projectsClient = new ProjectsClient(_mockProtoProjectsClient);
    }

    [Test]
    public async Task WhenClientIsInvoked_ThenProjectsAreReturned()
    {
        // arrange
        _mockProtoProjectsClient.GetProjectsAsync(Arg.Any<ProjectsRequest>())
            .Returns(
                new AsyncUnaryCall<ProjectsReply>(
                    Task.FromResult(new ProjectsReply
                    {
                        Projects = { ExpectedProjects }
                    }),
                    Task.FromResult(Metadata.Empty),
                    () => Status.DefaultSuccess,
                    () => Metadata.Empty,
                    () => { }
                )
            );

        // act
        var actualProjects = await _projectsClient.GetAsync().ConfigureAwait(false);

        // assert
        actualProjects.Should().BeEquivalentTo(ExpectedProjects);
    }

    private static readonly Project ExpectedProject = new()
    {
        Title = "some project",
        Description = "some description",
        ProjectUrl = "some url",
        Image = new Image
        {
            Url = "some url",
            ImgixUrl = "some other url"
        },
        Rank = 1,
        Tags = "some tags"
    };

    private static readonly List<Project> ExpectedProjects = [ExpectedProject];
}
