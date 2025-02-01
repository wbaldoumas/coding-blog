using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Components;
using Coding.Blog.Services;
using Microsoft.Net.Http.Headers;

namespace Coding.Blog.Extensions;

[ExcludeFromCodeCoverage]
internal static class WebApplicationExtensions
{
    private static readonly string[] _middlewareVaryHeaderValue = ["Accept-Encoding"];

    /// <summary>
    ///     Configures the given <see cref="WebApplication" /> with the necessary middleware.
    /// </summary>
    /// <param name="webApplication">The <see cref="WebApplication" /> to configure.</param>
    /// <returns>The configured <see cref="WebApplication" />.</returns>
    public static WebApplication Configure(this WebApplication webApplication)
    {
        webApplication
            .UseHealthChecks("/healthz")
            .HandleForwardedHeaders();

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
        webApplication.UseResponseCaching();
        webApplication.UseResponseCompression();
        webApplication.ConfigureResponseCache();
        webApplication.MapControllers();
        webApplication.MapStaticAssets();
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

    private static void HandleForwardedHeaders(this IApplicationBuilder webApplication)
    {
        webApplication
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
    }

    private static void ConfigureResponseCache(this IApplicationBuilder webApplication)
    {
        webApplication.Use(async (context, next) =>
        {
            context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(1200)
            };

            context.Response.Headers[HeaderNames.Vary] = _middlewareVaryHeaderValue;

            await next().ConfigureAwait(false);
        });
    }
}
