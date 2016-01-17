using KeySndr.Base.Commands;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GetAssemblyVersion_Tests
    {
        private GetAssemblyVersion cmd;

        [SetUp]
        public void Setup()
        {
            cmd = new GetAssemblyVersion();    
        }

        [Test]
        public void Test()
        {
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
    }
}
