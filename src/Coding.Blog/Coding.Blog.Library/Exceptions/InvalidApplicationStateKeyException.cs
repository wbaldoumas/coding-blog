using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Library.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class InvalidApplicationStateKeyException(string key)
    : ArgumentException($"Invalid application state key: {key}");
