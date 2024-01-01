using Coding.Blog.Library.Domain;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public interface IPersistentPostsService
{
    Task<IDictionary<string, Post>> GetAsync(PersistentComponentState applicationState, string stateKey);
}
