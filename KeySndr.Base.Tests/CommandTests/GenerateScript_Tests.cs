using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Common;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GenerateScript_Tests
    {
        private GenerateScript cmd;

        [SetUp]
        public void Setup()
        {
            cmd = new GenerateScript();         
        }

        [Test]
        public void Test()
        {
            cmd.Execute();
            Assert.IsNotNull(cmd);
            Assert.IsNotNull(cmd.Result);
            Assert.IsInstanceOf<ApiResult<InputScript>>(cmd.Result);
            Assert.IsTrue(cmd.Result.Success);
        }
    }
}
