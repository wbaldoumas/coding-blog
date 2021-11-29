using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Coding.Blog.Engine;
using Coding.Blog.Engine.Mappers;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Mappers;

[TestFixture]
public class PostLinkerTests
{
    [Test]
    public void PostLinker_links_expected_posts()
    {
        // arrange

        var firstPost = CreatePost("123");
        Thread.Sleep(10); // we are sleeping here to ensure that post "creation" times are different
        var secondPost = CreatePost("456");
        Thread.Sleep(10);
        var thirdPost = CreatePost("789");

        var expectedFirstPost = new Post(firstPost)
        {
            Previous = null,
            Next = secondPost
        };

        var expectedSecondPost = new Post(secondPost)
        {
            Previous = firstPost,
            Next = thirdPost
        };

        var expectedThirdPost = new Post(thirdPost)
        {
            Previous = secondPost,
            Next = null
        };

        // randomize the order
        var posts = new List<Post>
        {
            secondPost,
            thirdPost,
            firstPost
        };

        var postLinker = new PostLinker();

        // act
        var linkedPosts = postLinker.Link(posts);

        // assert
        linkedPosts.Select(post => post.DatePublished).Should().BeInDescendingOrder();

        linkedPosts.First(post => post.Id == firstPost.Id).Next.Should().BeEquivalentTo(expectedFirstPost.Next);
        linkedPosts.First(post => post.Id == firstPost.Id).Previous.Should().BeEquivalentTo(expectedFirstPost.Previous);

        linkedPosts.First(post => post.Id == secondPost.Id).Next.Should().BeEquivalentTo(expectedSecondPost.Next);
        linkedPosts.First(post => post.Id == secondPost.Id).Previous.Should().BeEquivalentTo(expectedSecondPost.Previous);

        linkedPosts.First(post => post.Id == thirdPost.Id).Next.Should().BeEquivalentTo(expectedThirdPost.Next);
        linkedPosts.First(post => post.Id == thirdPost.Id).Previous.Should().BeEquivalentTo(expectedThirdPost.Previous);
    }

    private static Post CreatePost(string id) =>
        new()
        {
            Id = id,
            Title = id,
            Content = id,
            DatePublished = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)),
            Hero = new Hero
            {
                ImgixUrl = id,
                Url = id
            }
        };
}