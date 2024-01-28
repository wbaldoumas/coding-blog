namespace Coding.Blog.Library.Services;

public interface IJSInteropService
{
    ValueTask ResetScrollPositionAsync();

    /// <summary>
    ///     This is a hack to make Giscus work nicely with a Blazor SPA. It's a workaround for the issue described
    ///     here: <see href="https://github.com/giscus/giscus/issues/357"/>, until a better solution within Blazor
    ///     is found, potentially implemented in <see href="https://github.com/Jisu-Woniu/giscus-blazor"/>.
    /// </summary>
    /// <returns> A <see cref="ValueTask"/> representing the asynchronous operation. </returns>
    ValueTask LoadGiscusAsync();
}
