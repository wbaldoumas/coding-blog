using Coding.Blog.Library.Domain;
using PostProto = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Library.Mappers;

public sealed class PostProtoToPostMapper : BaseMapper<PostProto, Post>
{
    public override Post Map(PostProto source) => new(
        source.Id,
        source.Slug,
        source.Title,
        source.Content,
        source.ReadingTime.ToTimeSpan(),
        source.DatePublished.ToDateTime(),
        source.Tags,
        new Hero(source.Hero.Url, source.Hero.ImgixUrl)
    );
}
