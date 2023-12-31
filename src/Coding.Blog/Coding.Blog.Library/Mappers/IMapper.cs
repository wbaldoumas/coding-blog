namespace Coding.Blog.Library.Mappers;

/// <summary>
///     Represents a mapper which can map from objects of type <typeparamref name="TSource"/> to
///     objects of type <typeparamref name="TTarget"/>.
/// </summary>
/// <typeparam name="TSource">The type of the source objects being mapped from</typeparam>
/// <typeparam name="TTarget">The type of the target objects being mapped to</typeparam>
public interface IMapper<in TSource, out TTarget>
{
    /// <summary>
    ///     Map from the given <paramref name="sources"/> to a list of objects of type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <param name="sources">The source objects being mapped from</param>
    /// <returns>A list of objects of type <typeparamref name="TTarget"/> that were mapped to from the <paramref name="sources"/></returns>
    IEnumerable<TTarget> Map(IEnumerable<TSource> sources);
}
