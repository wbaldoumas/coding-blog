using System.ServiceModel.Syndication;

namespace Coding.Blog.Services;

internal interface ISyndicationFeedService
{
    Task<SyndicationFeed> GetSyndicationFeed(string syndicationUrl);
}
