using System.ServiceModel.Syndication;
using Coding.Blog.Library.Domain;

namespace Coding.Blog.Utilities;

internal interface IPostToSyndicationItemMapper
{
    SyndicationItem Map(string syndicationUrl, Post post);
}
