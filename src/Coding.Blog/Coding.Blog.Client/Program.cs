using Coding.Blog.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.ConfigureServices(builder.Configuration);

await builder.Build()
    .RunAsync()
    .ConfigureAwait(false);
