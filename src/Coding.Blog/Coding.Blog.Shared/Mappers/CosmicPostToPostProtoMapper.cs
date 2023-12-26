using Coding.Blog.Shared.Protos;
using Coding.Blog.Shared.Records;
using Coding.Blog.Shared.Utilities;
using Google.Protobuf.WellKnownTypes;

namespace Coding.Blog.Shared.Mappers;

internal sealed class CosmicPostToPostProtoMapper(IReadTimeEstimator readTimeEstimator) : BaseMapper<CosmicPost, Post>
{
    public override Post Map(CosmicPost source) => new()
    {
        Id = source.Id,
        Slug = source.Slug,
        Title = source.Title,
        Content = source.Metadata.Markdown,
        ReadingTime = Duration.FromTimeSpan(readTimeEstimator.Estimate(source.Metadata.Markdown)),
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
