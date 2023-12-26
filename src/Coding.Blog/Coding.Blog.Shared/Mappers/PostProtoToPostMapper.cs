using Coding.Blog.Shared.Domain;
using PostProto = Coding.Blog.Shared.Protos.Post;

namespace Coding.Blog.Shared.Mappers;

public class PostProtoToPostMapper : BaseMapper<PostProto, Post>
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
