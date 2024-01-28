using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

internal static class ProjectsState
{
    public static IList<Project> Projects { get; set; } = new List<Project>();
}
