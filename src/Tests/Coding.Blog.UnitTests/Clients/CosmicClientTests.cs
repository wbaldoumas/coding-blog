using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Configurations;
using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Resilience;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Polly;

namespace Coding.Blog.UnitTests.Clients;

[TestFixture]
public sealed class CosmicClientTests
{
    private IOptions<CosmicConfiguration>? _configurationOptions;
    private CosmicConfiguration? _configuration;
    private ILogger<CosmicBooks>? _mockLogger;
    private IAsyncPolicy<CosmicBooks>? _resiliencePolicy;

    [SetUp]
    public void SetUp()
    {
        _configuration = new CosmicConfiguration
        {
            Endpoint = "https://api.cosmicjs.com/v2",
            BucketSlug = "TestBucket",
            ReadKey = "TestReadKey"
        };

        _configurationOptions = Substitute.For<IOptions<CosmicConfiguration>>();

        _configurationOptions.Value.Returns(_configuration);

        _mockLogger = Substitute.For<ILogger<CosmicBooks>>();
        _resiliencePolicy = ResiliencePolicyBuilder.Build<CosmicBooks>(TimeSpan.FromMilliseconds(1), 3, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task CosmicClient_responds_expected_response_when_api_returns_data()
    {
        // arrange
        using var httpTest = new HttpTest();

        httpTest.RespondWithJson(new CosmicBooks(
            new List<CosmicBook>
            {
                new("BookA", "ContentA", DateTime.Now, new CosmicBookMetadata("UrlA", new CosmicBookCoverMetadata("CoverA"))),
                new("BookB", "ContentB", DateTime.Now, new CosmicBookMetadata("UrlB", new CosmicBookCoverMetadata("CoverB"))),
                new("BookC", "ContentC", DateTime.Now, new CosmicBookMetadata("UrlC", new CosmicBookCoverMetadata("CoverC")))
            }
        ));

        var client = new CosmicClient<CosmicBooks>(_configurationOptions!, _mockLogger!, _resiliencePolicy!);

        // act
        var response = await client.GetAsync();

        // assert
        response.Should().NotBeNull();
        response.Books.Should().HaveCount(3);

        httpTest.ShouldHaveMadeACall();

        _mockLogger
            .DidNotReceiveWithAnyArgs()
            .LogError(default);
    }

    [Test]
    public async Task CosmicClient_throws_when_http_exception_is_thrown()
    {
        // arrange
        using var httpTest = new HttpTest();

        httpTest.RespondWith("bang!", 500);

        var client = new CosmicClient<CosmicBooks>(_configurationOptions!, _mockLogger!, _resiliencePolicy!);

        // act
        var act = async () => await client.GetAsync().ConfigureAwait(false);

        // assert
        await act.Should().ThrowAsync<FlurlHttpException>().ConfigureAwait(false);

        httpTest.ShouldHaveMadeACall();

        _mockLogger
            .ReceivedWithAnyArgs(1)
            .LogError(default);
    }
}