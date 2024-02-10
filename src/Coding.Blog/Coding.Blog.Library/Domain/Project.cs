using Coding.Blog.Library.Extensions;

namespace Coding.Blog.Library.Domain;

public sealed record Project(
    string Title,
    string Content,
    Image Image,
    string Url,
    int Rank,
    string Tags
)
{
    public const string Key = "Projects";

    public Lazy<IEnumerable<string>> DisplayTags => new(Tags.ToDisplayTags);
}
