using System;
using System.Collections.Generic;
using Coding.Blog.Engine;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Mappers;

[TestFixture]
public class PostMapperTests
{
    [Test]
    public void Map_generates_expected_post()
    {
        // arrange
        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var cosmicPost = new CosmicPost(
            "12345",
            "test-post",
            "Test Post",
            "This is some content talking about Test Post",
            mockDatePublished,
            new CosmicPostMetadata(new CosmicPostHero("https://google.com/a", "https://google.com/b"), "tag1, tag2, tag3")
        );

        var expectedPost = new Post
        {
            Id = "12345",
            Slug = "test-post",
            Title = "Test Post",
            Content = "This is some content talking about Test Post",
            DatePublished = Timestamp.FromDateTime(mockDatePublished),
            Tags = { new List<string> { "tag1", "tag2", "tag3" } },
            Hero = new Hero
            {
                Url = "https://google.com/a",
                ImgixUrl = "https://google.com/b"
            }
        };

        var mapper = new PostMapper();

        // act
        var post = mapper.Map(cosmicPost);

        // assert
        post.Should().BeEquivalentTo(expectedPost);
    }

    [Test]
    public void Map_generates_expected_posts()
    {
        // arrange
        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var cosmicPostA = new CosmicPost(
            "12345",
            "test-post-a",
            "Test Post A",
            "This is some content talking about Test Post A",
            mockDatePublished,
            new CosmicPostMetadata(new CosmicPostHero("https://google.com/a", "https://google.com/b"), string.Empty)
        );

        var cosmicPostB = new CosmicPost(
            "123456",
            "test-post-b",
            "Test Post B",
            "This is some content talking about Test Post B",
            mockDatePublished,
            new CosmicPostMetadata(new CosmicPostHero("https://google.com/b", "https://google.com/c"), "tag1, tag2, tag3")
        );

        var cosmicPosts = new List<CosmicPost>
        {
            cosmicPostA,
            cosmicPostB
        };

        var expectedPostA = new Post
        {
            Id = "12345",
            Slug = "test-post-a",
            Title = "Test Post A",
            Content = "This is some content talking about Test Post A",
            DatePublished = Timestamp.FromDateTime(mockDatePublished),
            Tags = { new List<string>() },
            Hero = new Hero
            {
                Url = "https://google.com/a",
                ImgixUrl = "https://google.com/b"
            }
        };

        var expectedPostB = new Post
        {
            Id = "123456",
            Slug = "test-post-b",
            Title = "Test Post B",
            Content = "This is some content talking about Test Post B",
            DatePublished = Timestamp.FromDateTime(mockDatePublished),
            Tags = { new List<string> { "tag1", "tag2", "tag3" } },
            Hero = new Hero
            {
                Url = "https://google.com/b",
                ImgixUrl = "https://google.com/c"
            }
        };

        var expectedPosts = new List<Post>
        {
            expectedPostA,
            expectedPostB
        };

        var mapper = new PostMapper();

        // act
        var posts = mapper.Map(cosmicPosts);

        // assert
        posts.Should().BeEquivalentTo(expectedPosts);
    }
}