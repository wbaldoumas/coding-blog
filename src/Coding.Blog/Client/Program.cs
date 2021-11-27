using Coding.Blog.Client;
using Coding.Blog.Engine;
using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Configurations;
using Coding.Blog.Engine.Resilience;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(services =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
    var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });

    return new Books.BooksClient(channel);
});

builder.Services.AddSingleton<IResilientBooksClient, ResilientBooksClient>(services =>
{
    var booksClient = services.GetRequiredService<Books.BooksClient>();
    var logger = services.GetRequiredService<ILogger<Book>>();
    var resiliencePolicy = ResiliencePolicyBuilder.Build<IEnumerable<Book>>(
        TimeSpan.FromMilliseconds(ResilienceConfiguration.DefaultMedianFirstRetryDelayMilliseconds),
        ResilienceConfiguration.DefaultRetryCount,
        TimeSpan.FromMilliseconds(ResilienceConfiguration.DefaultTimeToLiveMilliseconds)
    );

    return new ResilientBooksClient(booksClient, logger, resiliencePolicy);
});

await builder.Build().RunAsync();
