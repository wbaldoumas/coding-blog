using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Polly;

namespace Coding.Blog.Engine.Clients;

[ExcludeFromCodeCoverage]
public class ResilientBooksClient : IResilientBooksClient
{
    private readonly Books.BooksClient _booksClient;
    private readonly ILogger<Book> _logger;
    private readonly IAsyncPolicy<IEnumerable<Book>> _resiliencePolicy;

    public ResilientBooksClient(
        Books.BooksClient booksClient,
        ILogger<Book> logger,
        IAsyncPolicy<IEnumerable<Book>> resiliencePolicy)
    {
        _booksClient = booksClient;
        _logger = logger;
        _resiliencePolicy = resiliencePolicy;
    }

    public async Task<IEnumerable<Book>> GetAsync()
    {
        try
        {
            return await _resiliencePolicy.ExecuteAsync(async () =>
            {
                var booksReply = await _booksClient.GetBooksAsync(new BooksRequest());

                return booksReply.Books;

            });
        }
        catch (Exception exception)
        {
            _logger.LogError($"Unable to retrieve books from backend service: {exception.Message}!");

            throw;
        }
    }
}