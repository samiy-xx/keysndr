using KeySndr.Base.Commands;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class ExecuteInputAction_Tests
    {
        private Mock<IActionProcessor> actionProcessorMock;
        private IActionProcessor actionProcessor;
        private ExecuteInputAction cmd;

        [SetUp]
        public void Setup()
        {
            actionProcessorMock = new Mock<IActionProcessor>();
            actionProcessor = actionProcessorMock.Object;
        }

        [Test]
        public void Test()
        {
            var action = TestFactory.CreateTestInputAction();
            var container = InputActionExecutionContainer.Wrap(action);
            cmd = new ExecuteInputAction(actionProcessor, container);
            cmd.Execute().Wait(1000);
            var result = cmd.Result;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            actionProcessorMock.Verify(a => a.Process(), Times.Once);
        }
    }
}
