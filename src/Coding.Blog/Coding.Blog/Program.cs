using Coding.Blog.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", optional: true);
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

await app.Configure()
    .RunAsync()
    .ConfigureAwait(false);
