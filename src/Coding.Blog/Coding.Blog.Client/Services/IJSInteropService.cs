namespace Coding.Blog.Client.Services;

public interface IJSInteropService
{
    ValueTask ChangeUrlAsync(string path);

    ValueTask ResetScrollPositionAsync();
}
