using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Coding.Blog.Services;
using FluentAssertions;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class ProjectsServiceTests
{
    private ICosmicClient<CosmicProject> _mockClient = null!;

    private IMapper _mockMapper = null!;

    private ProjectsService _projectsService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = Substitute.For<ICosmicClient<CosmicProject>>();
        _mockMapper = Substitute.For<IMapper>();
        _projectsService = new ProjectsService(_mockClient, _mockMapper);
    }

    [Test]
    public async Task WhenProjectsServiceIsInvoked_ThenProjectsAreReturned()
    {
        // arrange
        _mockClient
            .GetAsync()
            .Returns(ExpectedCosmicProjects);

        _mockMapper
            .Map<CosmicProject, Project>(ExpectedCosmicProjects)
            .Returns(ExpectedProjects);

        var request = new ProjectsRequest();
        var serverCallContext = Substitute.For<ServerCallContext>();

        // act
        var actualProjects = await _projectsService.GetProjects(request, serverCallContext);

        // assert
        actualProjects.Projects.Should().BeEquivalentTo(ExpectedProjects);
    }

    private static readonly CosmicProject ExpectedCosmicProject = new(
        Title: "some title",
        Metadata: new CosmicProjectMetadata(
            Content: "some description",
            Rank: 1,
            Url: "some GitHub url",
            Image: new CosmicImage(
                Url: "some other url"
            ),
            Tags: "some,tags"
        )
    );

    private static readonly List<CosmicProject> ExpectedCosmicProjects = [ExpectedCosmicProject];

    private static readonly Project ExpectedProject = new()
    {
        Title = "some title",
        Content = "some description",
        Image = new Image
        {
            Url = "some other url"
        },
        Url = "some project url",
        Rank = 1
    };

    private static readonly List<Project> ExpectedProjects = [ExpectedProject];
}
