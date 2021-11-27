using System.Runtime.CompilerServices;
using Coding.Blog.Engine.Records;
using Google.Protobuf.WellKnownTypes;

[assembly:InternalsVisibleTo("Coding.Blog.UnitTests")]
namespace Coding.Blog.Engine.Mappers;

internal class BookMapper : BaseMapper<CosmicBook, Book>
{
    public override Book Map(CosmicBook source) => new()
    {
        Title = source.Title,
        Content = source.Content,
        PurchaseUrl = source.Metadata.PurchaseUrl,
        CoverImageUrl = source.Metadata.Cover.Url,
        DatePublished = Timestamp.FromDateTime(source.PublishedAt)
    };
}
