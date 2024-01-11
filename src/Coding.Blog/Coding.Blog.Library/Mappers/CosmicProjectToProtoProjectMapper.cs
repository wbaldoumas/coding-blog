using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Records;

namespace Coding.Blog.Library.Mappers;

public sealed class CosmicProjectToProtoProjectMapper : BaseMapper<CosmicProject, Project>
{
    public override Project Map(CosmicProject source) => new()
    {
        Title = source.Title,
        Description = source.Metadata.Description,
        Image = new Image
        {
            Url = source.Metadata.Image.Url,
            ImgixUrl = source.Metadata.Image.ImgixUrl
        },
        ProjectUrl = source.Metadata.GitHubUrl,
        Rank = source.Metadata.Rank,
        Tags =
        {
            source.Metadata.Tags.Trim().Length > 0
                ? source.Metadata.Tags.Split(",").Select(tag => tag.Trim())
                : new List<string>()
        }
    };
}
