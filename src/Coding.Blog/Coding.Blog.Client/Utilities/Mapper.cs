using Coding.Blog.Library.Utilities;
using Google.Protobuf.WellKnownTypes;
using Riok.Mapperly.Abstractions;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Client.Utilities;

/// <summary>
///     A mapper for mapping between different types.
/// </summary>
[Mapper]
internal sealed partial class Mapper : IMapper
{
    public partial TTarget Map<TSource, TTarget>(TSource source);

    public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source) => source.Select(Map<TSource, TTarget>);

    [MapperIgnoreTarget("Next")]
    [MapperIgnoreTarget("Previous")]
    private partial Post ProtoPostToPost(ProtoPost protoPost);

    private partial Book ProtoBookToBook(ProtoBook protoBook);

    private partial Project ProtoProjectToProject(ProtoProject protoProject);

    private static TimeSpan DurationToTimeSpan(Duration duration) => duration.ToTimeSpan();

    private static DateTime TimestampToDateTime(Timestamp timestamp) => timestamp.ToDateTime();
}
