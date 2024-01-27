using System.ServiceModel.Syndication;
using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Utilities;

public interface IPostToSyndicationItemMapper
{
    SyndicationItem Map(string syndicationUrl, Post post);
}
