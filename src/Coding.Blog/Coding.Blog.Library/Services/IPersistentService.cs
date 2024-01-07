using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public interface IPersistentService<T>
{
    Task<T> GetAsync(PersistentComponentState applicationState, string stateKey);
}
