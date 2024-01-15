namespace Coding.Blog.Library.Utilities;

public interface IMapper
{
    TTarget Map<TSource, TTarget>(TSource source);

    IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source);
}
