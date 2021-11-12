using Autofac;
using Autofac.Extensions.DependencyInjection;
using Coding.Blog.Server.Configurations;
using Coding.Blog.Server.HostedServices;
using Coding.Blog.Server.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddHealthChecks();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var applicationLifetimeConfiguration = new ApplicationLifetimeConfiguration();
builder.Configuration.GetSection(ApplicationLifetimeConfiguration.Key).Bind(applicationLifetimeConfiguration);
builder.Services.AddSingleton(applicationLifetimeConfiguration);
builder.Services.AddHostedService<ApplicationLifetimeService>();
builder.Services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(
        applicationLifetimeConfiguration.ApplicationShutdownTimeoutSeconds
    );
});

builder.Services.AddGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Host.ConfigureContainer<ContainerBuilder>(_ =>
{
    // register AutoFac modules here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHealthChecks("/healthz");
app.UseForwardedHeaders();
app.Use(async (context, next) =>
{
    if (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps)
    {
        await next();
    }
    else
    {
        var queryString = context.Request.QueryString.HasValue
            ? context.Request.QueryString.Value
            : string.Empty;
        var https = "https://" + context.Request.Host + context.Request.Path + queryString;

        context.Response.Redirect(https, true);
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

app.MapGrpcService<WeatherService>().EnableGrpcWeb();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();