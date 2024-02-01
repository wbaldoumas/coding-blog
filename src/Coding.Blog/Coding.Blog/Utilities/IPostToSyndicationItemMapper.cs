using System.ServiceModel.Syndication;
using Coding.Blog.Library.Domain;

namespace Coding.Blog.Utilities;

/// <summary>
///     Maps a <see cref="Post"/> to a <see cref="SyndicationItem"/>.
/// </summary>
internal interface IPostToSyndicationItemMapper
{
    SyndicationItem Map(string syndicationUrl, Post post);
}
