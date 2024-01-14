using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

public static class ApplicationState
{
    public static void SetState<T>(IEnumerable<T>? state, string key)
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

    public static IList<T> GetState<T>(string key)
    {
        return key switch
        {
            ProjectsState.Key => ProjectsState.Projects as IList<T> ?? new List<T>(),
            PostsState.Key => PostsState.Posts as IList<T> ?? new List<T>(),
            BooksState.Key => BooksState.Books as IList<T> ?? new List<T>(),
            _ => throw new ArgumentException("Invalid key", nameof(key))
        };
    }
}
