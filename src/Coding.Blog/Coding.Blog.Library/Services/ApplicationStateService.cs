using Coding.Blog.Library.Domain;
using Coding.Blog.Library.State;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public class ApplicationStateService<T>(IBlogService<IEnumerable<T>> blogService) : IApplicationStateService<T>
{
    public async Task<IList<T>> GetAsync(PersistentComponentState persistentComponentState, string key)
    {
        if (persistentComponentState.TryTakeFromJson<IList<T>>(key, out var persistedComponentState))
        {
            SetState(persistedComponentState, key);
        }

        var persistedState = GetState(key);

        if (persistedState.Any())
        {
            return persistedState;
        }

        var serverState = await LoadStateAsync().ConfigureAwait(false);

        SetState(serverState, key);

        return serverState;
    }

    private static void SetState(IEnumerable<T>? state, string key)
    {
        switch (key)
        {
            case ProjectsState.Key:
                ProjectsState.Projects = state as IList<Project> ?? new List<Project>();
                break;
            case PostsState.Key:
                PostsState.Posts = state as IList<Post> ?? new List<Post>();
                break;
            case BooksState.Key:
                BooksState.Books = state as IList<Book> ?? new List<Book>();
                break;
            default:
                throw new ArgumentException("Invalid key", nameof(key));
        }
    }

    private static IList<T> GetState(string key)
    {
        return key switch
        {
            ProjectsState.Key => ProjectsState.Projects as IList<T> ?? new List<T>(),
            PostsState.Key => PostsState.Posts as IList<T> ?? new List<T>(),
            BooksState.Key => BooksState.Books as IList<T> ?? new List<T>(),
            _ => throw new ArgumentException("Invalid key", nameof(key))
        };
    }

    private async Task<IList<T>> LoadStateAsync()
    {
        var serverState = await blogService.GetAsync().ConfigureAwait(false);

        return serverState.ToList();
    }
}
