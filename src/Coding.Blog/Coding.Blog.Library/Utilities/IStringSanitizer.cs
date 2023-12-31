namespace Coding.Blog.Library.Utilities;

/// <summary>
///     Sanitizes a given string, with the assumption that the content is markdown.
/// </summary>
public interface IStringSanitizer
{
    /// <summary>
    ///     Sanitizes the given string, with the assumption that the content is markdown.
    /// </summary>
    /// <param name="content">The string content to sanitize.</param>
    /// <returns>The sanitized string content.</returns>
    string Sanitize(string content);
}
