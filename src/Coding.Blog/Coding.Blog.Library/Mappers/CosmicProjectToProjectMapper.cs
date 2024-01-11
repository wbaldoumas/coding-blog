using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Records;

namespace Coding.Blog.Library.Mappers;

public sealed class CosmicProjectToProjectMapper : BaseMapper<CosmicProject, Project>
{
    public override Project Map(CosmicProject source)
    {
        return new Project(
            source.Title,
            source.Metadata.Description,
            new Image(source.Metadata.Image.Url, source.Metadata.Image.ImgixUrl),
            source.Metadata.GitHubUrl,
            source.Metadata.Rank,
            source.Metadata.Tags.Trim().Length > 0
                ? source.Metadata.Tags.Split(",").Select(tag => tag.Trim())
                : new List<string>()
        );
    }
}
