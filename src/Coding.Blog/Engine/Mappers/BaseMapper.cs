namespace Coding.Blog.Engine.Mappers;

internal abstract class BaseMapper<TSource, TTarget> : IMapper<TSource, TTarget>
{
    public abstract TTarget Map(TSource source);

    public IEnumerable<TTarget> Map(IEnumerable<TSource> sources) => sources.Select(Map);
}
