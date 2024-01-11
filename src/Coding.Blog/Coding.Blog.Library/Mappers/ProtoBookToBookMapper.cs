using Coding.Blog.Library.Domain;
using Book = Coding.Blog.Library.Domain.Book;
using ProtoBook = Coding.Blog.Library.Protos.Book;

namespace Coding.Blog.Library.Mappers;

public sealed class ProtoBookToBookMapper : BaseMapper<ProtoBook, Book>
{
    public override Book Map(ProtoBook source) => new(
        source.Title,
        source.Content,
        new Image(source.Image.Url, source.Image.ImgixUrl),
        source.PurchaseUrl,
        source.DatePublished.ToDateTime()
    );
}
