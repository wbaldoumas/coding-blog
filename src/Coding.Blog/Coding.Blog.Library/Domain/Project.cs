using Coding.Blog.Library.Extensions;

namespace Coding.Blog.Library.Domain;

public sealed record Project(
    string Title,
    string Description,
    Image Image,
    string ProjectUrl,
    int Rank,
    string Tags
)
{
    public Lazy<IEnumerable<string>> DisplayTags => new(Tags.ToDisplayTags);
}
