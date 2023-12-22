namespace Coding.Blog.Shared.Domain;

public sealed record Project(
    string Title,
    string Description,
    string ImageUrl,
    string ProjectUrl,
    int Rank,
    IEnumerable<string> Tags
);
