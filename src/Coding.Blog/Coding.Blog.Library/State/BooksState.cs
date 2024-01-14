using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

internal static class BooksState
{
    public const string Key = "Books";

    public static IList<Book> Books { get; set; } = new List<Book>();
}
