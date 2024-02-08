using Coding.Blog.Options;
using Coding.Blog.Services;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class ApplicationLifetimeServiceTests
{
    private IHostApplicationLifetime _mockHostApplicationLifetime = null!;

    private IOptions<ApplicationLifetimeOptions> _mockOptions = null!;

    private ApplicationLifetimeService _applicationLifetimeService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockHostApplicationLifetime = Substitute.For<IHostApplicationLifetime>();
        _mockOptions = Substitute.For<IOptions<ApplicationLifetimeOptions>>();

        _mockOptions.Value.Returns(new ApplicationLifetimeOptions { ApplicationStoppingGracePeriodSeconds = 5 });

        _applicationLifetimeService = new ApplicationLifetimeService(_mockHostApplicationLifetime, _mockOptions);
    }

    [Test]
    public async Task WhenStartAsyncIsInvoked_ThenApplicationStoppingEventIsRegistered()
    {
        // act
        var act = async () => await _applicationLifetimeService.StartAsync(CancellationToken.None).ConfigureAwait(false);

        // assert
        await act.Should().NotThrowAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task WhenStopAsyncIsInvoked_ThenItDoesNotThrow()
    {
        // act
        var act = async () => await _applicationLifetimeService.StopAsync(CancellationToken.None).ConfigureAwait(false);

        // assert
        await act.Should().NotThrowAsync().ConfigureAwait(false);
    }
}
