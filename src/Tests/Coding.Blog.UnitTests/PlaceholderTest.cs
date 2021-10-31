using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.UnitTests
{
    /// <summary>
    ///     Placeholder test to confirm CI works as expected until I add more tests.
    /// </summary>
    [TestFixture]
    public class PlaceholderTest
    {
        [Test]
        public void TestFoo()
        {
            const string foo = "foo";

            foo.Should().Be("foo");
            foo.Should().NotBe("bar");
        }
    }
}