namespace Coding.Blog.Library.Utilities;

/// <summary>
///     A simple mapper for mapping objects from one type to another.
/// </summary>
public interface IMapper
{
    TTarget Map<TSource, TTarget>(TSource source);

    IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source);
}
