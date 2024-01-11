using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Records;

namespace Coding.Blog.Library.Mappers;

public sealed class CosmicBookToBookMapper : BaseMapper<CosmicBook, Book>
{
    public override Book Map(CosmicBook source) => new(
        source.Title,
        source.Content,
        new Image(source.Metadata.Image.Url, source.Metadata.Image.ImgixUrl),
        source.Metadata.PurchaseUrl,
        source.DatePublished
    );
}
