using Coding.Blog.Library.Domain;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Library.Mappers;

public sealed class ProtoProjectToProjectMapper : BaseMapper<ProtoProject, Project>
{
    public override Project Map(ProtoProject source) => new(
        source.Title,
        source.Description,
        new Image(source.Image.Url, source.Image.ImgixUrl),
        source.ProjectUrl,
        source.Rank,
        source.Tags
    );
}
