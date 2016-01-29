using System.Collections.Generic;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class SaveInputScript_Tests
    {
        private Mock<IScriptProvider> scriptProviderMock;
        private Mock<IStorageProvider> storageProviderMock;

        private IScriptProvider scriptProvider;
        private IStorageProvider storageProvider;

        private SaveInputScript cmd;
        private InputScript existingScript;
        private InputScript nonExistingScript;

        [SetUp]
        public void Setup()
        {
            existingScript = TestFactory.CreateTestInputScript();
            nonExistingScript = TestFactory.CreateTestInputScript();
            nonExistingScript.FileName = "somethingelse";

            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProviderMock.Setup(s => s.Scripts).Returns(Scripts);
            scriptProvider = scriptProviderMock.Object;

            storageProviderMock = new Mock<IStorageProvider>();
            storageProvider = storageProviderMock.Object;
        }

        [Test]
        public async void ExistingScriptWithSameFileNameIsSaved()
        {
            cmd = new SaveInputScript(storageProvider, scriptProvider, existingScript);
            await cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            scriptProviderMock.Verify(v => v.Scripts, Times.AtLeastOnce);
            scriptProviderMock.Verify(v => v.AddOrUpdate(It.Is<InputScript>(s => s.Equals(existingScript)), true), Times.Once);
            storageProviderMock.Verify(v => v.UpdateScript(It.Is<InputScript>(s => s.Equals(existingScript)), It.IsAny<InputScript>()), Times.Never);
            storageProviderMock.Verify(v => v.SaveScript(It.Is<InputScript>(s => s.Equals(existingScript))), Times.Once);
            storageProviderMock.Verify(v => v.LoadAllSourceFiles(It.Is<InputScript>(s => s.Equals(existingScript))), Times.Once);
        }

        [Test]
        public async void ExistingScriptWithChangedFileNameIsUpdated()
        {
            var existingWithNewFileName = TestFactory.Copy(existingScript);
            existingWithNewFileName.FileName = "changed";

            cmd = new SaveInputScript(storageProvider, scriptProvider, existingWithNewFileName);
            await cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            scriptProviderMock.Verify(v => v.Scripts, Times.AtLeastOnce);
            scriptProviderMock.Verify(v => v.AddOrUpdate(It.Is<InputScript>(s => s.Equals(existingWithNewFileName)), true), Times.Once);
            storageProviderMock.Verify(v => v.UpdateScript(It.Is<InputScript>(s => s.Equals(existingWithNewFileName)), It.IsAny<InputScript>()), Times.Once);
            storageProviderMock.Verify(v => v.SaveScript(It.Is<InputScript>(s => s.Equals(existingWithNewFileName))), Times.Never);
            storageProviderMock.Verify(v => v.LoadAllSourceFiles(It.Is<InputScript>(s => s.Equals(existingWithNewFileName))), Times.Once);
        }

        [Test]
        public async void NonExistingScriptIsSaved()
        {
            cmd = new SaveInputScript(storageProvider, scriptProvider, nonExistingScript);
            await cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            scriptProviderMock.Verify(v => v.Scripts, Times.AtLeastOnce);
            scriptProviderMock.Verify(v => v.AddOrUpdate(It.Is<InputScript>(s => s.Equals(nonExistingScript)), true), Times.Once);
            storageProviderMock.Verify(v => v.UpdateScript(It.Is<InputScript>(s => s.Equals(nonExistingScript)), It.IsAny<InputScript>()), Times.Never);
            storageProviderMock.Verify(v => v.SaveScript(It.Is<InputScript>(s => s.Equals(nonExistingScript))), Times.Once);
            storageProviderMock.Verify(v => v.LoadAllSourceFiles(It.Is<InputScript>(s => s.Equals(nonExistingScript))), Times.Once);
        }

        private IEnumerable<InputScript> Scripts()
        {
            return new List<InputScript>
            {
                existingScript
            };
        } 
    }
}
