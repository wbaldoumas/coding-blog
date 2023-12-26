using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Mappers;

internal sealed class CosmicBookToBookMapper : BaseMapper<CosmicBook, Book>
{
    public override Book Map(CosmicBook source) => new(
        source.Title,
        source.Content,
        source.Metadata.Cover.Url,
        source.Metadata.PurchaseUrl,
        source.DatePublished
    );
}
