namespace Coding.Blog.Library.Domain;

public sealed record Book(
    string Title,
    string Content,
    string CoverImageUrl,
    string PurchaseUrl,
    DateTime DatePublished
);
