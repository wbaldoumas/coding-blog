using Coding.Blog.Library.Services;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace Coding.Blog.Controllers;

public sealed class RssFeedController(ISyndicationFeedService syndicationFeedService) : ControllerBase
{
    [HttpGet]
    [Route("feed.rss")]
    [ResponseCache(Duration = 1200)]
    public async Task<IActionResult> GetRssFeedAsync()
    {
        var syndicationUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

        var syndicationFeed = await syndicationFeedService.GetSyndicationFeed(syndicationUrl).ConfigureAwait(false);

        using var memoryStream = new MemoryStream();

        await WriteRssInfoToStreamAsync(memoryStream, syndicationFeed);

        return File(memoryStream.ToArray(), "application/rss+xml; charset=utf-8");
    }

    private static async Task WriteRssInfoToStreamAsync(Stream stream, SyndicationFeed syndicationFeed)
    {
        await using var xmlWriter = XmlWriter.Create(stream, XmlWriterSettings);

        var rssFormatter = new Rss20FeedFormatter(syndicationFeed, false);
        rssFormatter.WriteTo(xmlWriter);

        await xmlWriter.FlushAsync();
    }

    private static readonly XmlWriterSettings XmlWriterSettings = new()
    {
        Encoding = Encoding.UTF8,
        NewLineHandling = NewLineHandling.Entitize,
        Indent = true,
        Async = true
    };
}
