using KeySndr.Base.Commands;
using KeySndr.Common;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GenerateInputConfiguration_Tests
    {
        private const int Actions = 10;
        private GenerateInputConfiguration cmd;

        [SetUp]
        public void Setup()
        {
            cmd = new GenerateInputConfiguration(Actions);
        }

        [Test]
        public void Generates_InputConfiguration()
        {
            cmd.Execute();

            Assert.IsNotNull(cmd.Result);
            Assert.IsTrue(cmd.Result.Success);
            Assert.IsNotNull(cmd.Result.Content);
            Assert.IsInstanceOf<InputConfiguration>(cmd.Result.Content);
        }

        [Test]
        public void Has_Count_Actions()
        {
            cmd.Execute();
            Assert.AreEqual(Actions, cmd.Result.Content.Actions.Count);
        }
    }
}
