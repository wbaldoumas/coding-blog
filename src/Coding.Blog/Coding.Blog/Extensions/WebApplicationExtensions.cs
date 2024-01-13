using Coding.Blog.Components;
using Coding.Blog.Library.Services;

namespace Coding.Blog.Extensions;

internal static class WebApplicationExtensions
{
    /// <summary>
    ///     Configures the given <see cref="WebApplication" /> with the necessary middleware.
    /// </summary>
    /// <param name="webApplication">The <see cref="WebApplication" /> to configure.</param>
    /// <returns>The configured <see cref="WebApplication" />.</returns>
    public static WebApplication Configure(this WebApplication webApplication)
    {
        webApplication
            .UseHealthChecks("/healthz")
            .UseForwardedHeaders()
            .Use((context, next) =>
            {
                if (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps)
                {
                    return next();
                }

                var queryString = context.Request.QueryString.HasValue
                    ? context.Request.QueryString.Value
                    : string.Empty;

                var https = "https://" + context.Request.Host + context.Request.Path + queryString;

                context.Response.Redirect(https, permanent: true);

                return Task.CompletedTask;
            });

        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseWebAssemblyDebugging();
        }
        else
        {
            webApplication.UseExceptionHandler("/Error", createScopeForErrors: true);
            webApplication.UseHsts();
        }

        webApplication.UseHttpsRedirection();
        webApplication.UseStaticFiles();
        webApplication.UseAntiforgery();
        webApplication.UseGrpcWeb();
        webApplication.MapGrpcService<PostsService>().EnableGrpcWeb();
        webApplication.MapGrpcService<BooksService>().EnableGrpcWeb();
        webApplication.MapGrpcService<ProjectsService>().EnableGrpcWeb();

        webApplication.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client.Pages.Blog).Assembly);

        return webApplication;
    }
}
