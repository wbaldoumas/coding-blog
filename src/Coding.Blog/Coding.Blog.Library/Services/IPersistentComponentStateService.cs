using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

/// <summary>
///    A service for retrieving data from <see cref="PersistentComponentState"/> or static state.
/// </summary>
/// <typeparam name="T">The type of object to retrieve</typeparam>
public interface IPersistentComponentStateService<T>
{
    Task<IList<T>> GetAsync(PersistentComponentState persistentComponentState, string key);
}
