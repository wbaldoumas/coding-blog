namespace Coding.Blog.Library.Domain;

public sealed record Book(
    string Title,
    string Content,
    string Author,
    Image Image,
    string Url,
    DateTime DatePublished
)
{
    public const string Key = "Books";
}
