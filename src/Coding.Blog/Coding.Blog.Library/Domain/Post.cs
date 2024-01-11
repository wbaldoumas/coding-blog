namespace Coding.Blog.Library.Domain;

public sealed record Post(
    string Id,
    string Slug,
    string Title,
    string Content,
    TimeSpan ReadingTime,
    DateTime DatePublished,
    IEnumerable<string> Tags,
    Image Image,
    Post? Next = null,
    Post? Previous = null
);
