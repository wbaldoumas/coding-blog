using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Records;

namespace Coding.Blog.Library.Mappers;

public sealed class CosmicBookToBookMapper : BaseMapper<CosmicBook, Book>
{
    public override Book Map(CosmicBook source) => new(
        source.Title,
        source.Content,
        source.Metadata.Cover.Url,
        source.Metadata.PurchaseUrl,
        source.DatePublished
    );
}
