namespace Coding.Blog.Library.Domain;

public sealed record Project(
    string Title,
    string Description,
    Image Image,
    string ProjectUrl,
    int Rank,
    IEnumerable<string> Tags
);
