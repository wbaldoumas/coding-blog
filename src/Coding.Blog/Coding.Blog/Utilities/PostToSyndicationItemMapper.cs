using Coding.Blog.Library.Domain;
using System.ServiceModel.Syndication;
using System.Xml.Linq;

namespace Coding.Blog.Utilities;

/// <summary>
///    Maps a <see cref="Post"/> to a <see cref="SyndicationItem"/>.
/// </summary>
/// <param name="stringSanitizer">A <see cref="IStringSanitizer"/> for sanitizing strings.</param>
internal sealed class PostToSyndicationItemMapper(IStringSanitizer stringSanitizer) : IPostToSyndicationItemMapper
{
    public SyndicationItem Map(string syndicationUrl, Post post)
    {
        var postUrl = new Uri(syndicationUrl + $"/post/{post.Slug}");
        var postDescription = stringSanitizer.Sanitize(post.Description);
        var postDate = new DateTimeOffset(post.DatePublished);

        var syndicationItem = new SyndicationItem(post.Title, postDescription, postUrl, post.Id, postDate)
        {
            Summary = new TextSyndicationContent(postDescription, TextSyndicationContentKind.Plaintext),
            BaseUri = postUrl,
            PublishDate = postDate,
            LastUpdatedTime = postDate,
            ElementExtensions = { new XElement("image", post.Image.Url) }
        };

        foreach (var tag in post.DisplayTags.Value)
        {
            syndicationItem.Categories.Add(new SyndicationCategory(tag));
        }

        return syndicationItem;
    }
}
