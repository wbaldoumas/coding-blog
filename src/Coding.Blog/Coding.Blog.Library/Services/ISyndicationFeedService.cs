using System.ServiceModel.Syndication;

namespace Coding.Blog.Library.Services;

public interface ISyndicationFeedService
{
    Task<SyndicationFeed> GetSyndicationFeed(string syndicationUrl);
}
