﻿using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Records;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Library.Mappers;

public sealed class CosmicPostToPostMapper(IReadTimeEstimator readTimeEstimator) : BaseMapper<CosmicPost, Post>
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