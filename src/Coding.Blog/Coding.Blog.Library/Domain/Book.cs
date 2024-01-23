namespace Coding.Blog.Library.Domain;

public sealed record Book(
    string Title,
    string Content,
    string Author,
    Image Image,
    string PurchaseUrl,
    DateTime DatePublished
);
