using System.ServiceModel.Syndication;

namespace Coding.Blog.Services;

public interface ISyndicationFeedService
{
    Task<SyndicationFeed> GetSyndicationFeed(string syndicationUrl);
}
