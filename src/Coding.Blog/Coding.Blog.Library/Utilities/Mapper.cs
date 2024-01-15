using Coding.Blog.Library.DataTransfer;
using Google.Protobuf.WellKnownTypes;
using Riok.Mapperly.Abstractions;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Library.Utilities;

[Mapper]
public partial class Mapper(IReadTimeEstimator readTimeEstimator) : IMapper
{
    public partial TTarget Map<TSource, TTarget>(TSource source);

    public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source) => source.Select(Map<TSource, TTarget>);

    [MapProperty("Metadata.PurchaseUrl", "PurchaseUrl")]
    [MapProperty("Metadata.Image", "Image")]
    private partial Book CosmicBookToBook(CosmicBook cosmicBook);

    [MapProperty("Metadata.PurchaseUrl", "PurchaseUrl")]
    [MapProperty("Metadata.Image", "Image")]
    private partial ProtoBook CosmicBookToProtoBook(CosmicBook cosmicBook);

    private partial Book ProtoBookToBook(ProtoBook protoBook);

    [MapProperty("Metadata.Markdown", "Content")]
    [MapProperty("Metadata.Markdown", "ReadingTime")]
    [MapProperty("Metadata.Tags", "Tags")]
    [MapProperty("Metadata.Image", "Image")]
    [MapperIgnoreTarget("DisplayTags")]
    [MapperIgnoreTarget("Next")]
    [MapperIgnoreTarget("Previous")]
    private partial Post CosmicPostToPost(CosmicPost cosmicPost);

    [MapProperty("Metadata.Markdown", "Content")]
    [MapProperty("Metadata.Markdown", "ReadingTime")]
    [MapProperty("Metadata.Tags", "Tags")]
    [MapProperty("Metadata.Image", "Image")]
    private partial ProtoPost CosmicPostToProtoPost(CosmicPost cosmicPost);

    [MapperIgnoreTarget("Next")]
    [MapperIgnoreTarget("Previous")]
    private partial Post ProtoPostToPost(ProtoPost protoPost);

    [MapProperty("Metadata.Description", "Description")]
    [MapProperty("Metadata.Image", "Image")]
    [MapProperty("Metadata.GitHubUrl", "ProjectUrl")]
    [MapProperty("Metadata.Rank", "Rank")]
    [MapProperty("Metadata.Tags", "Tags")]
    [MapperIgnoreTarget("DisplayTags")]
    private partial Project CosmicProjectToProject(CosmicProject project);

    [MapProperty("Metadata.Description", "Description")]
    [MapProperty("Metadata.Image", "Image")]
    [MapProperty("Metadata.GitHubUrl", "ProjectUrl")]
    [MapProperty("Metadata.Rank", "Rank")]
    [MapProperty("Metadata.Tags", "Tags")]
    private partial ProtoProject CosmicProjectToProtoProject(CosmicProject project);

    private partial Project ProtoProjectToProject(ProtoProject protoProject);

    private static TimeSpan DurationToTimeSpan(Duration duration) => duration.ToTimeSpan();

    private static Timestamp DateTimeToTimestamp(DateTime dateTime) => Timestamp.FromDateTime(dateTime);

    private static DateTime TimestampToDateTime(Timestamp timestamp) => timestamp.ToDateTime();

    private TimeSpan ContentToTimeSpan(string content) => readTimeEstimator.Estimate(content);

    private Duration ContentToDuration(string content) => readTimeEstimator.Estimate(content).ToDuration();
}
