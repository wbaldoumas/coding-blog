using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Records;
using Google.Protobuf.WellKnownTypes;

namespace Coding.Blog.Library.Mappers;

public sealed class CosmicBookToProtoBookMapper : BaseMapper<CosmicBook, Book>
{
    public override Book Map(CosmicBook source) => new()
    {
        Title = source.Title,
        Content = source.Content,
        CoverImageUrl = source.Metadata.Cover.Url,
        PurchaseUrl = source.Metadata.PurchaseUrl,
        DatePublished = Timestamp.FromDateTime(source.DatePublished),
    };
}
