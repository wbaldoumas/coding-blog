using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public interface IApplicationStateService<T>
{
    Task<IList<T>> GetAsync(PersistentComponentState persistentComponentState, string key);
}
