using Coding.Blog.Library.Domain;
using ProtoPost = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Library.Mappers;

public sealed class ProtoPostToPostMapper : BaseMapper<ProtoPost, Post>
{
    public override Post Map(ProtoPost source) => new(
        source.Id,
        source.Slug,
        source.Title,
        source.Content,
        source.ReadingTime.ToTimeSpan(),
        source.DatePublished.ToDateTime(),
        source.Tags,
        new Image(source.Image.Url, source.Image.ImgixUrl),
        source.Next is null ? null : Map(source.Next),
        source.Previous is null ? null : Map(source.Previous)
    );
}
