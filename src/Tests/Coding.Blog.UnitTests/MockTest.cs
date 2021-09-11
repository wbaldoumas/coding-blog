using System;
using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.UnitTests
{
    /// <summary>
    ///     A temp/mock test put in place to test getting unit test coverage reports generated.
    /// </summary>
    [TestFixture]
    public class MockTest
    {
        [Test]
        public void Test_foo()
        {
            // arrange
            var foo = new Random().Next(1, 1000);

            // act
            var isGreaterThanOne = foo > 1;

            isGreaterThanOne.Should().BeTrue();
        }
    }
}
