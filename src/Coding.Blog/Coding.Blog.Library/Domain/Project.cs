namespace Coding.Blog.Library.Domain;

public sealed record Project(
    string Title,
    string Description,
    string ImageUrl,
    string ProjectUrl,
    int Rank,
    IEnumerable<string> Tags
);
