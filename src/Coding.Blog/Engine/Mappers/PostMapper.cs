using Coding.Blog.Engine.Records;
using Google.Protobuf.WellKnownTypes;

namespace Coding.Blog.Engine.Mappers;

internal class PostMapper : BaseMapper<CosmicPost, Post>
{
    public override Post Map(CosmicPost source) => new()
    {
        Id = source.Id,
        Slug = source.Slug,
        Title = source.Title,
        Content = source.Content,
        DatePublished = Timestamp.FromDateTime(source.DatePublished),
        Tags =
        {
            source.Metadata.Tags.Trim().Length > 0
                ? source.Metadata.Tags.Split(",").Select(tag => tag.Trim())
                : new List<string>()
        },
        Hero = new Hero
        {
            Url = source.Metadata.Hero.Url,
            ImgixUrl = source.Metadata.Hero.ImgixUrl
        }
    };
}