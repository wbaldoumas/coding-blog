using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Coding.Blog.Client.Pages;
using Coding.Blog.Components;
using Coding.Blog.Extensions;
using Coding.Blog.Shared.Extensions;
using Coding.Blog.Shared.Modules;
using Coding.Blog.Shared.Services;
using Markdig;
using Markdown.ColorCode;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", optional: true);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddHealthChecks();

builder.Services
    .AddCosmicConfiguration(builder.Configuration)
    .AddResilienceConfiguration(builder.Configuration)
    .AddApplicationLifetimeService(builder.Configuration)
    .AddQuartzJobs(builder.Configuration)
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddGrpc();

builder.Services
    .AddBlazorise()
    .AddEmptyProviders()
    .AddFontAwesomeIcons();

builder.Services.AddSignalR(hubOptions => {
    hubOptions.MaximumReceiveMessageSize = 1024000;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(1);
    hubOptions.EnableDetailedErrors = true;
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(30);
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
    hubOptions.StreamBufferCapacity = 20;
    hubOptions.StatefulReconnectBufferSize = 1024000;
});

builder.Services.AddSingleton(_ => new MarkdownPipelineBuilder().UseAdvancedExtensions().UseColorCode().Build());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => { containerBuilder.RegisterModule<CodingBlogModule>(); });

var app = builder.Build();

app
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

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseGrpcWeb();
app.MapGrpcService<ProtoPostsService>().EnableGrpcWeb();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Blog).Assembly);

await app
    .RunAsync()
    .ConfigureAwait(false);
