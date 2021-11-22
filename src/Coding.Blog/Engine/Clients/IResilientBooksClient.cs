namespace Coding.Blog.Engine.Clients;

/// <summary>
///     Retrieves <see cref="Book"/>s from the backend in a resilient way, with retries
///     and client-side caching in place.
/// </summary>
public interface IResilientBooksClient
{
    /// <summary>
    ///     Retrieves <see cref="Book"/>s from the backend in a resilient way, with retries
    ///     and client-side caching in place.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> to await, whose result is an <see cref="IEnumerable{T}"/> of <see cref="Book"/>s</returns>
    Task<IEnumerable<Book>> GetAsync();
}