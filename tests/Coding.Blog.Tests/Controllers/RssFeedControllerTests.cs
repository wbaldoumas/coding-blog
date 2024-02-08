using Coding.Blog.Controllers;
using Coding.Blog.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NUnit.Framework;
using System.ServiceModel.Syndication;

namespace Coding.Blog.Tests.Controllers;

[TestFixture]
internal sealed class RssFeedControllerTests
{
    private ISyndicationFeedService _mockSyndicationFeedService = null!;

    private RssFeedController _rssFeedController = null!;

    [SetUp]
    public void SetUp()
    {
        _mockSyndicationFeedService = Substitute.For<ISyndicationFeedService>();
        _rssFeedController = new RssFeedController(_mockSyndicationFeedService);
    }

    [Test]
    public async Task WhenGetRssFeedAsyncIsInvoked_ThenRssFeedIsReturned()
    {
        // arrange
        _mockSyndicationFeedService.GetSyndicationFeed(Arg.Any<string>()).Returns(new SyndicationFeed());

        var mockHttpRequest = Substitute.For<HttpRequest>();
        var mockHttpContext = Substitute.For<HttpContext>();

        mockHttpRequest.Scheme.Returns("https");
        mockHttpRequest.Host.Returns(new HostString("localhost"));
        mockHttpRequest.PathBase.Returns(new PathString("/"));

        mockHttpContext.Request.Returns(mockHttpRequest);

        _rssFeedController.ControllerContext = new ControllerContext(
            new ActionContext(
                mockHttpContext,
                new RouteData(),
                new ControllerActionDescriptor()
            )
        );

        // act
        var result = await _rssFeedController.GetRssFeedAsync().ConfigureAwait(false);

        // assert
        result.Should().BeOfType<FileContentResult>();

        await _mockSyndicationFeedService
            .Received(1)
            .GetSyndicationFeed(Arg.Any<string>())
            .ConfigureAwait(false);
    }
}
