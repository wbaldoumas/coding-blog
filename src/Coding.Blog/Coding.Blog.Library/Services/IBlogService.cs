namespace Coding.Blog.Library.Services;

/// <summary>
///     A generic interface representing a service which can retrieve domain objects for the blog app
/// </summary>
/// <typeparam name="TDomainObject">The type of the domain object to retrieve.</typeparam>
public interface IBlogService<TDomainObject>
{
    Task<IEnumerable<TDomainObject>> GetAsync();
}
