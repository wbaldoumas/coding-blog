﻿using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Utilities;
using Google.Protobuf.WellKnownTypes;
using Riok.Mapperly.Abstractions;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Utilities;

[Mapper]
internal sealed partial class Mapper(IReadTimeEstimator readTimeEstimator) : IMapper
{
    public partial TTarget Map<TSource, TTarget>(TSource source);

    public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source) => source.Select(Map<TSource, TTarget>);

    [MapProperty("Metadata.PurchaseUrl", "PurchaseUrl")]
    [MapProperty("Metadata.Image", "Image")]
    [MapProperty("Metadata.Content", "Content")]
    [MapProperty("Metadata.Author", "Author")]
    private partial Book CosmicBookToBook(CosmicBook cosmicBook);

    [MapProperty("Metadata.PurchaseUrl", "PurchaseUrl")]
    [MapProperty("Metadata.Image", "Image")]
    [MapProperty("Metadata.Content", "Content")]
    [MapProperty("Metadata.Author", "Author")]
    private partial ProtoBook CosmicBookToProtoBook(CosmicBook cosmicBook);

    [MapProperty("Metadata.Markdown", "Content")]
    [MapProperty("Metadata.Markdown", "ReadingTime")]
    [MapProperty("Metadata.Tags", "Tags")]
    [MapProperty("Metadata.Image", "Image")]
    [MapProperty("Metadata.Description", "Description")]
    [MapperIgnoreTarget("DisplayTags")]
    [MapperIgnoreTarget("Next")]
    [MapperIgnoreTarget("Previous")]
    private partial Post CosmicPostToPost(CosmicPost cosmicPost);

    [MapProperty("Metadata.Markdown", "Content")]
    [MapProperty("Metadata.Markdown", "ReadingTime")]
    [MapProperty("Metadata.Tags", "Tags")]
    [MapProperty("Metadata.Image", "Image")]
    [MapProperty("Metadata.Description", "Description")]
    private partial ProtoPost CosmicPostToProtoPost(CosmicPost cosmicPost);

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

    private static Timestamp DateTimeToTimestamp(DateTime dateTime) => Timestamp.FromDateTime(dateTime);

    private TimeSpan ContentToTimeSpan(string content) => readTimeEstimator.Estimate(content);

    private Duration ContentToDuration(string content) => readTimeEstimator.Estimate(content).ToDuration();
}