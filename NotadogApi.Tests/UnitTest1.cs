using NUnit.Framework;

namespace NotadogApi.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        private bool result;

        // Runs before every test.
        [SetUp]
        public void SetUp()
        {
            result = true;
        }

        [Test]
        public void Mehtod_Conditions_ExpectedAction()
        {
            Assert.True(result, "result should be true");
        }
    }
}
