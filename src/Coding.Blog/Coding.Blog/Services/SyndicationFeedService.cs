using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Services;
using Coding.Blog.Options;
using Coding.Blog.Utilities;
using Microsoft.Extensions.Options;
using System.ServiceModel.Syndication;

namespace Coding.Blog.Services;

/// <summary>
///     A service for generating a <see cref="SyndicationFeed"/> from a given URL.
/// </summary>
/// <param name="applicationInfoOptions">The application info options.</param>
/// <param name="blogPostService">The blog post service to retrieve post information to inject into the feed.</param>
/// <param name="mapper">A mapper for mapping a post to a syndication feed item.</param>
internal sealed class SyndicationFeedService(
    IOptions<ApplicationInfoOptions> applicationInfoOptions,
    IBlogService<Post> blogPostService,
    IPostToSyndicationItemMapper mapper
) : ISyndicationFeedService
{
    private readonly ApplicationInfoOptions _applicationInfoOptions = applicationInfoOptions.Value;

    public async Task<SyndicationFeed> GetSyndicationFeed(string syndicationUrl)
    {
        var posts = (await blogPostService.GetAsync().ConfigureAwait(false)).ToList();

        return new SyndicationFeed(_applicationInfoOptions.Title, _applicationInfoOptions.Description, new Uri(syndicationUrl))
        {
            BaseUri = new Uri(syndicationUrl),
            Items = posts.Select(post => mapper.Map(syndicationUrl, post)),
            LastUpdatedTime = new DateTimeOffset(posts.Max(post => post.DatePublished))
        };
    }
}
