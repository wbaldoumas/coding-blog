using Coding.Blog.Library.Extensions;

namespace Coding.Blog.Library.Domain;

public sealed record Post(
    string Id,
    string Slug,
    string Title,
    string Content,
    string Description,
    TimeSpan ReadingTime,
    DateTime DatePublished,
    string Tags,
    Image Image,
    Post? Next = null,
    Post? Previous = null
)
{
    public Lazy<IEnumerable<string>> DisplayTags => new(Tags.ToDisplayTags);
}
