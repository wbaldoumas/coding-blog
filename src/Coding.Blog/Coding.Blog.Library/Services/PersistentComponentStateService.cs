using Coding.Blog.Library.State;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

/// <summary>
///    A service for retrieving data from <see cref="PersistentComponentState"/> or static state.
/// </summary>
/// <typeparam name="T">The type of object to retrieve</typeparam>
/// <param name="blogService">A <see cref="IBlogService{T}"/> for retrieving the data from the server when not found in state.</param>
public sealed class PersistentComponentStateService<T>(IBlogService<T> blogService) : IPersistentComponentStateService<T>
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
