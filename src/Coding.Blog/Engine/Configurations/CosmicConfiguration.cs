namespace Coding.Blog.Engine.Configurations;

public class CosmicConfiguration
{
    public static string Key => "Cosmic";

    public string BucketSlug { get; set; } = string.Empty;

    public string ReadKey { get; set; } = string.Empty;
}