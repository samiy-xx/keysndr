using KeySndr.Base.Commands;
using KeySndr.Common;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GenerateInputAction_Tests
    {
        private GenerateInputAction cmd;

        [SetUp]
        public void Setup()
        {
            cmd = new GenerateInputAction();    
        }

        [Test]
        public void Generates_Action()
        {
            cmd.Execute();
            Assert.IsNotNull(cmd.Result);
            Assert.IsInstanceOf<ApiResult<InputAction>>(cmd.Result);
        }
    }
}
