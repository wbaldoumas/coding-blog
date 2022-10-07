using Autofac;
using Autofac.Extensions.DependencyInjection;
using Coding.Blog.Engine.Configurations;
using Coding.Blog.Engine.Modules;
using Coding.Blog.Engine.Services;
using Coding.Blog.Server.Configurations;
using Coding.Blog.Server.HostedServices;
using Microsoft.AspNetCore.HttpOverrides;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", optional: true);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddHealthChecks();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.Configure<CosmicConfiguration>(
    builder.Configuration.GetSection(CosmicConfiguration.Key)
);

builder.Services.Configure<ResilienceConfiguration>(
    builder.Configuration.GetSection(ResilienceConfiguration.Key)
);

builder.Services.Configure<ApplicationLifetimeConfiguration>(
    builder.Configuration.GetSection(ApplicationLifetimeConfiguration.Key)
);

builder.Services.AddHostedService<ApplicationLifetimeService>();

builder.Services.Configure<HostOptions>(options =>
{
    var applicationShutdownTimeoutSeconds = int.Parse(
        builder.Configuration[$"{ApplicationLifetimeConfiguration.Key}:ApplicationShutdownTimeoutSeconds"],
        CultureInfo.InvariantCulture
    );

    options.ShutdownTimeout = TimeSpan.FromSeconds(applicationShutdownTimeoutSeconds);
});

builder.Services.AddGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<CodingBlogModule>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHealthChecks("/healthz");
app.UseForwardedHeaders();
app.Use(async (context, next) =>
{
    if (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps)
    {
        await next().ConfigureAwait(false);
    }
    else
    {
        var queryString = context.Request.QueryString.HasValue
            ? context.Request.QueryString.Value
            : string.Empty;
        var https = "https://" + context.Request.Host + context.Request.Path + queryString;

        context.Response.Redirect(https, permanent: true);
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseGrpcWeb();

app.MapGrpcService<BooksService>().EnableGrpcWeb();
app.MapGrpcService<PostsService>().EnableGrpcWeb();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync().ConfigureAwait(false);