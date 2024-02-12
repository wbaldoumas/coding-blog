using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.DataTransfer.PostProcessors;
using Coding.Blog.Options;
using Coding.Blog.Utilities;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Polly;

namespace Coding.Blog.Tests.Clients;

[TestFixture]
internal sealed class CosmicClientTests : IDisposable
{
    private HttpTest _httpTest = null!;

    private IOptions<CosmicOptions> _mockOptions = null!;

    private ILogger<CosmicBook> _mockLogger = null!;

    private IAsyncPolicy<IEnumerable<CosmicBook>> _resiliencePolicy = null!;

    private ICosmicObjectPostProcessor<CosmicBook> _mockCosmicBookPostProcessor = null!;

    private CosmicClient<CosmicBook> _cosmicClient = null!;

    private static readonly TimeSpan TestMedianFirstRetryDelay = TimeSpan.FromTicks(1);

    private const int TestRetryCount = 3;

    private static readonly TimeSpan TestTimeToLive = TimeSpan.FromSeconds(5);

    [SetUp]
    public void SetUp()
    {
        _httpTest = new HttpTest();
        _mockOptions = Substitute.For<IOptions<CosmicOptions>>();

        _mockOptions.Value.Returns(
            new CosmicOptions
            {
                Endpoint = "https://example.com/cosmic",
                BucketSlug = "some-bucket-slug",
                ReadKey = "some-read-key"
            }
        );

        _mockLogger = Substitute.For<ILogger<CosmicBook>>();

        _resiliencePolicy = ResiliencePolicyBuilder.Build<IEnumerable<CosmicBook>>(
            TestMedianFirstRetryDelay,
            TestRetryCount,
            TestTimeToLive
        );

        _mockCosmicBookPostProcessor = Substitute.For<ICosmicObjectPostProcessor<CosmicBook>>();

        _mockCosmicBookPostProcessor
            .Process(Arg.Any<IEnumerable<CosmicBook>>())
            .Returns(ExpectedCosmicBooks);

        _cosmicClient = new CosmicClient<CosmicBook>(
            _mockOptions, 
            _mockLogger, 
            _resiliencePolicy,
            _mockCosmicBookPostProcessor
        );
    }

    [TearDown]
    public void TearDown() => _httpTest.Dispose();

    [Test]
    public async Task WhenClientInvokesSuccessfulRequest_ThenBooksAreReturned()
    {
        // arrange
        _httpTest.RespondWithJson(new CosmicObjects<CosmicBook>(Objects: ExpectedCosmicBooks));

        // act
        var cosmicBooks = await _cosmicClient.GetAsync().ConfigureAwait(false);

        // assert
        cosmicBooks.Should().BeEquivalentTo(ExpectedCosmicBooks);

        _httpTest.ShouldHaveCalled($"{_mockOptions.Value.Endpoint}*").Times(1);
    }

    [Test]
    public async Task WhenClientInvokesFailedRequest_ThenRetriesArePerformedAndBooksAreReturned()
    {
        // arrange
        _httpTest
            .RespondWith("server error", 500)
            .RespondWith("another server error", 500)
            .RespondWithJson(new CosmicObjects<CosmicBook>(Objects: ExpectedCosmicBooks));

        // act
        var cosmicBooks = await _cosmicClient.GetAsync().ConfigureAwait(false);

        // assert
        cosmicBooks.Should().BeEquivalentTo(ExpectedCosmicBooks);

        _httpTest.ShouldHaveCalled($"{_mockOptions.Value.Endpoint}*").Times(TestRetryCount);
    }

    [Test]
    public async Task WhenAllClientRequestsFail_ThenRetriesArePerformedAndExceptionIsThrown()
    {
        // arrange
        _httpTest
            .RespondWith("server error", 500)
            .RespondWith("another server error", 500)
            .RespondWith("yet another server error", 500);

        // act
        var act = async () => await _cosmicClient.GetAsync().ConfigureAwait(false);

        // assert
        await act.Should().ThrowAsync<FlurlHttpException>().ConfigureAwait(false);
    }

    private static readonly CosmicBook ExpectedCosmicBook = new(
        Title: "some book",
        DatePublished: DateTime.UtcNow,
        Metadata: new CosmicBookMetadata(
            Url: "some url",
            Image: new CosmicImage(
                Url: "some imgix url"
            ),
            Content: "some content",
            Author: "some author"
        )
    );

    private static readonly List<CosmicBook> ExpectedCosmicBooks = [ExpectedCosmicBook];

    public void Dispose() => _httpTest.Dispose();
}
