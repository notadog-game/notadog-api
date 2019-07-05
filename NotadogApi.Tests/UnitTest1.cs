using System;
using Xunit;

namespace NotadogApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var result = true;
            Assert.True(result, "result should not be true");
        }
    }
}
