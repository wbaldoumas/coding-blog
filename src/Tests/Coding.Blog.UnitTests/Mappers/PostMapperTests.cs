using System;
using System.Collections.Generic;
using Coding.Blog.Engine;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Utilities;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Mappers;

[TestFixture]
public class PostMapperTests
{
    private IReadTimeEstimator _mockReadTimeEstimator;

    [SetUp]
    public void SetUp() => _mockReadTimeEstimator = Substitute.For<IReadTimeEstimator>();

    [Test]
    public void Map_generates_expected_post()
    {
        // arrange
        _mockReadTimeEstimator
            .Estimate(Arg.Any<string>())
            .Returns(TimeSpan.MaxValue);

        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var cosmicPost = new CosmicPost(
            "12345",
            "test-post",
            "Test Post",
            mockDatePublished,
            new CosmicPostMetadata(
                new CosmicPostHero("https://google.com/a", "https://google.com/b"),
                "tag1, tag2, tag3",
                "This is some markdown for Test Post"
            )
        );

        var expectedPost = new Post
        {
            Id = "12345",
            Slug = "test-post",
            Title = "Test Post",
            Content = "This is some markdown for Test Post",
            DatePublished = Timestamp.FromDateTime(mockDatePublished),
            Tags = { new List<string> { "tag1", "tag2", "tag3" } },
            ReadingTime = Duration.FromTimeSpan(TimeSpan.MaxValue),
            Hero = new Hero
            {
                Url = "https://google.com/a",
                ImgixUrl = "https://google.com/b"
            }
        };

        var mapper = new PostMapper(_mockReadTimeEstimator);

        // act
        var post = mapper.Map(cosmicPost);

        // assert
        post.Should().BeEquivalentTo(expectedPost);
    }

    [Test]
    public void Map_generates_expected_posts()
    {
        // arrange
        _mockReadTimeEstimator
            .Estimate(Arg.Any<string>())
            .Returns(TimeSpan.MaxValue);

        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var cosmicPostA = new CosmicPost(
            "12345",
            "test-post-a",
            "Test Post A",
            mockDatePublished,
            new CosmicPostMetadata(
                new CosmicPostHero("https://google.com/a", "https://google.com/b"),
                string.Empty,
                "This is some markdown talking about Test Post A"
            )
        );

        var cosmicPostB = new CosmicPost(
            "123456",
            "test-post-b",
            "Test Post B",
            mockDatePublished,
            new CosmicPostMetadata(
                new CosmicPostHero("https://google.com/b", "https://google.com/c"),
                "tag1, tag2, tag3",
                "This is some markdown talking about Test Post B"
            )
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
            Content = "This is some markdown talking about Test Post A",
            DatePublished = Timestamp.FromDateTime(mockDatePublished),
            Tags = { new List<string>() },
            ReadingTime = Duration.FromTimeSpan(TimeSpan.MaxValue),
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
            Content = "This is some markdown talking about Test Post B",
            DatePublished = Timestamp.FromDateTime(mockDatePublished),
            Tags = { new List<string> { "tag1", "tag2", "tag3" } },
            ReadingTime = Duration.FromTimeSpan(TimeSpan.MaxValue),
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

        var mapper = new PostMapper(_mockReadTimeEstimator);

        // act
        var posts = mapper.Map(cosmicPosts);

        // assert
        posts.Should().BeEquivalentTo(expectedPosts);
    }
}