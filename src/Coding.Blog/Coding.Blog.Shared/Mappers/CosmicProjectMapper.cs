using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Mappers;

internal sealed class CosmicProjectMapper : BaseMapper<CosmicProject, Project>
{
    public override Project Map(CosmicProject source)
    {
        return new Project(
            source.Title,
            source.Metadata.Description,
            source.Metadata.Hero.Url,
            source.Metadata.GitHubUrl,
            source.Metadata.Rank,
            source.Metadata.Tags.Trim().Length > 0
                ? source.Metadata.Tags.Split(",").Select(tag => tag.Trim())
                : new List<string>()
        );
    }
}
