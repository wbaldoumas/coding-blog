using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Records;
using Coding.Blog.Shared.Utilities;

namespace Coding.Blog.Shared.Mappers;

internal sealed class CosmicPostToPostMapper(IReadTimeEstimator readTimeEstimator) : BaseMapper<CosmicPost, Post>
{
    public override Post Map(CosmicPost source) => new(
        source.Id,
        source.Slug,
        source.Title,
        source.Metadata.Markdown,
        readTimeEstimator.Estimate(source.Metadata.Markdown),
        source.DatePublished,
        source.Metadata.Tags.Trim().Length > 0
            ? source.Metadata.Tags.Split(",").Select(tag => tag.Trim())
            : new List<string>(),
        new Hero(
            source.Metadata.Hero.Url,
            source.Metadata.Hero.ImgixUrl
        )
    );
}
