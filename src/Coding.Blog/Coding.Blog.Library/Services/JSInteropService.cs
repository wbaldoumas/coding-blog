using Microsoft.JSInterop;

namespace Coding.Blog.Library.Services;

public sealed class JSInteropService(IJSRuntime jsRuntime) : IJSInteropService
{
    public ValueTask ResetScrollPositionAsync() => jsRuntime.InvokeVoidAsync("resetScrollPosition");

    public async ValueTask LoadGiscusAsync()
    {
        await jsRuntime.InvokeVoidAsync("removeGiscusFrame").ConfigureAwait(false);
        await jsRuntime.InvokeVoidAsync("addGiscusFrame").ConfigureAwait(false);
    }
}
