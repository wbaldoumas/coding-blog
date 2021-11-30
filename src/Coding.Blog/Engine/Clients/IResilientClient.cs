namespace Coding.Blog.Engine.Clients;

/// <summary>
///     Retrieves <see cref="T"/>s from the backend in a resilient way, with retries
///     and client-side caching in place.
/// </summary>
/// <typeparam name="T">The type of object being retrieved</typeparam>
public interface IResilientClient<T>
{
    /// <summary>
    ///     Retrieves <see cref="Book"/>s from the backend in a resilient way, with retries
    ///     and client-side caching in place.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> to await, whose result is an <see cref="IEnumerable{T}"/> of <see cref="T"/>s</returns>
    Task<IEnumerable<T>> GetAsync();
}