namespace Coding.Blog.Shared.Domain;

public sealed record Book(
    string Title,
    string Content,
    string CoverImageUrl,
    string PurchaseUrl,
    DateTime DatePublished
);
