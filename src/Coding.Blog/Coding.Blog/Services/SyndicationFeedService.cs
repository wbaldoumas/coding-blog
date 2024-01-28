using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Services;
using Coding.Blog.Options;
using Coding.Blog.Utilities;
using Microsoft.Extensions.Options;
using System.ServiceModel.Syndication;

namespace Coding.Blog.Services;

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
