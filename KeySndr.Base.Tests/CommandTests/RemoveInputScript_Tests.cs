using System;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class RemoveInputScript_Tests
    {
        private Mock<IScriptProvider> scriptProviderMock;
        private Mock<IStorageProvider> storageProviderMock;

        private IScriptProvider scriptProvider;
        private IStorageProvider storageProvider;

        private RemoveInputScript cmd;
        private InputScript okScript;
        private InputScript notOkScript;

        [SetUp]
        public void Setup()
        {
            okScript = TestFactory.CreateTestInputScript();
            notOkScript = TestFactory.CreateTestInputScript();

            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProviderMock.Setup(s => s.RemoveScript(It.Is<InputScript>(v => v.Equals(notOkScript))))
                .Throws<Exception>();
            scriptProvider = scriptProviderMock.Object;

            storageProviderMock = new Mock<IStorageProvider>();
            storageProvider = storageProviderMock.Object;
        }

        [Test]
        public void SuccessReturnsTrue()
        {
            cmd = new RemoveInputScript(scriptProvider, storageProvider, okScript);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            scriptProviderMock.Verify(v => v.RemoveScript(It.Is<InputScript>(s => s.Equals(okScript))), Times.Once);
            storageProviderMock.Verify(v => v.RemoveScript(It.Is<InputScript>(s => s.Equals(okScript))), Times.Once);
        }

        [Test]
        public void SuccessReturnsFalse()
        {
            cmd = new RemoveInputScript(scriptProvider, storageProvider, notOkScript);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);

            scriptProviderMock.Verify(v => v.RemoveScript(It.Is<InputScript>(s => s.Equals(notOkScript))), Times.Once);
            storageProviderMock.Verify(v => v.RemoveScript(It.Is<InputScript>(s => s.Equals(notOkScript))), Times.Never);
        }
    }
}
