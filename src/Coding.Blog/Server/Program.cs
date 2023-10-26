using Autofac;
using Autofac.Extensions.DependencyInjection;
using Coding.Blog.Engine.Extensions;
using Coding.Blog.Engine.Modules;
using Coding.Blog.Engine.Services;
using Coding.Blog.Server.Configurations;
using Coding.Blog.Server.HostedServices;
using Microsoft.AspNetCore.HttpOverrides;
using System.Globalization;
using Coding.Blog.Server.Extensions;
using Coding.Blog.Engine.Jobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", optional: true);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddHealthChecks();

builder.Services
    .AddCosmicConfiguration(builder.Configuration)
    .AddResilienceConfiguration(builder.Configuration)
    .AddApplicationLifetimeConfiguration(builder.Configuration)
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
    .Configure<HostOptions>(options =>
    {
        var applicationShutdownTimeoutSeconds = int.Parse(
            builder.Configuration["ApplicationLifetime:ApplicationShutdownTimeoutSeconds"]!,
            CultureInfo.InvariantCulture
        );

        options.ShutdownTimeout = TimeSpan.FromSeconds(applicationShutdownTimeoutSeconds);
    })
    .AddHostedService<ApplicationLifetimeService>();

builder.Services.AddQuartz(serviceCollectionQuartzConfigurator =>
{
    serviceCollectionQuartzConfigurator
        .ConfigureJob<PostsWarmingJob>(builder.Configuration)
        .ConfigureJob<BooksWarmingJob>(builder.Configuration);
});

builder.Services.AddQuartzHostedService();
builder.Services.AddGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<CodingBlogModule>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app
    .UseHealthChecks("/healthz")
    .UseForwardedHeaders()
    .Use(async (context, next) =>
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
    app.UseExceptionHandler("/Error").UseHsts();
}

app.UseHttpsRedirection()
    .UseBlazorFrameworkFiles()
    .UseStaticFiles()
    .UseRouting()
    .UseGrpcWeb();

app.MapGrpcService<BooksService>().EnableGrpcWeb();
app.MapGrpcService<PostsService>().EnableGrpcWeb();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app
    .RunAsync()
    .ConfigureAwait(false);