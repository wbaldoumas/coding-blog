namespace Coding.Blog.DataTransfer.PostProcessors;

internal interface ICosmicObjectPostProcessor<T>
{
    /// <summary>
    ///     Post-processes the cosmic objects. This allows for manipulating the cosmic objects after they have been
    ///     retrieved from the Cosmic API, but before they are cached in memory.
    /// </summary>
    /// <param name="cosmicObjects">The cosmic objects to post-process.</param>
    /// <returns>The post-processed cosmic objects.</returns>
    IEnumerable<T> Process(IEnumerable<T> cosmicObjects);
}
