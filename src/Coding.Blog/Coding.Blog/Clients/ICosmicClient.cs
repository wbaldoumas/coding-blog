﻿namespace Coding.Blog.Clients;

/// <summary>
///     A simple client for retrieving objects from the Cosmic API.
/// </summary>
/// <typeparam name="T">The type of object to retrieve</typeparam>
internal interface ICosmicClient<T>
{
    /// <summary>
    ///     Get the <see cref="T"/> object from the Cosmic API.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> to await, whose result is a <see cref="T"/></returns>
    Task<IEnumerable<T>> GetAsync();
}
