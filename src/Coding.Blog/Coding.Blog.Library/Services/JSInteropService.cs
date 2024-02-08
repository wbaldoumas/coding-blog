using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Coding.Blog.Library.Services;

/// <summary>
///    A service encapsulating JS interop calls.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class JSInteropService(IJSRuntime jsRuntime) : IJSInteropService
{
    public ValueTask ResetScrollPositionAsync() => jsRuntime.InvokeVoidAsync("resetScrollPosition");

    public async ValueTask LoadGiscusAsync()
    {
        await jsRuntime.InvokeVoidAsync("removeGiscusFrame").ConfigureAwait(false);
        await jsRuntime.InvokeVoidAsync("addGiscusFrame").ConfigureAwait(false);
    }
}
