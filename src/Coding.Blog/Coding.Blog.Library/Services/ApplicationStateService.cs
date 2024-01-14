using Coding.Blog.Library.State;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public sealed class ApplicationStateService<T>(IBlogService<IEnumerable<T>> blogService) : IApplicationStateService<T>
{
    public async Task<IList<T>> GetAsync(PersistentComponentState persistentComponentState, string key)
    {
        if (persistentComponentState.TryTakeFromJson<IList<T>>(key, out var persistedComponentState))
        {
            ApplicationState.SetState(persistedComponentState, key);
        }

        var persistedState = ApplicationState.GetState<T>(key);

        if (persistedState.Any())
        {
            return persistedState;
        }

        var serverState = await LoadStateAsync().ConfigureAwait(false);

        ApplicationState.SetState(serverState, key);

        return serverState;
    }

    private async Task<IList<T>> LoadStateAsync()
    {
        var serverState = await blogService.GetAsync().ConfigureAwait(false);

        return serverState.ToList();
    }
}
