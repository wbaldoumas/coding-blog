namespace Coding.Blog.Library.Domain;

public sealed record Book(
    string Title,
    string Content,
    Image Image,
    string PurchaseUrl,
    DateTime DatePublished
);
