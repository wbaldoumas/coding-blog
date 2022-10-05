using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Utilities;
using Google.Protobuf.WellKnownTypes;

namespace Coding.Blog.Engine.Mappers;

internal sealed class PostMapper : BaseMapper<CosmicPost, Post>
{
    private readonly IReadTimeEstimator _readTimeEstimator;

    public PostMapper(IReadTimeEstimator readTimeEstimator) => _readTimeEstimator = readTimeEstimator;

    public override Post Map(CosmicPost source) => new()
    {
        Id = source.Id,
        Slug = source.Slug,
        Title = source.Title,
        Content = source.Metadata.Markdown,
        ReadingTime = Duration.FromTimeSpan(_readTimeEstimator.Estimate(source.Metadata.Markdown)),
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