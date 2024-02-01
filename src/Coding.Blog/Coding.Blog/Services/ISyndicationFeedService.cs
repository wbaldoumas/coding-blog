using System.ServiceModel.Syndication;

namespace Coding.Blog.Services;

/// <summary>
///     A service for generating a <see cref="SyndicationFeed"/> from a given URL.
/// </summary>
public interface ISyndicationFeedService
{
    Task<SyndicationFeed> GetSyndicationFeed(string syndicationUrl);
}
