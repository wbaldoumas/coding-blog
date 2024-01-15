using Microsoft.JSInterop;

namespace Coding.Blog.Client.Services;

public sealed class JSInteropService(IJSRuntime jsRuntime) : IJSInteropService
{
    public ValueTask ChangeUrlAsync(string path) => jsRuntime.InvokeVoidAsync("ChangeUrl", path);

    public ValueTask ResetScrollPositionAsync() => jsRuntime.InvokeVoidAsync("resetScrollPosition");
}
