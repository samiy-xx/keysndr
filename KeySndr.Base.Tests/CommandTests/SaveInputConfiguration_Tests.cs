using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    class SaveInputConfiguration_Tests
    {
        private IInputConfigProvider inputConfigProvider;
        private IAppConfigProvider appConfigProvider;
        private IStorageProvider storageProvider;

        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private Mock<IAppConfigProvider> appConfigProviderMock;
        private Mock<IStorageProvider> storageProviderMock;
        private Mock<IFileSystemUtils> fileSystemUtilsMock;

        private SaveInputConfiguration cmd;
        private InputConfiguration newConfiguration;
        private InputConfiguration oldConfiguration;

        [SetUp]
        public void Setup()
        {
            fileSystemUtilsMock = new Mock<IFileSystemUtils>();
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.IsAny<string>())).Returns(true);

            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProvider = inputConfigProviderMock.Object;

            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProvider = appConfigProviderMock.Object;

            storageProviderMock = new Mock<IStorageProvider>();
            storageProviderMock.Setup(s => s.SaveInputConfiguration(It.IsAny<InputConfiguration>()));
            storageProviderMock.Setup(s => s.UpdateInputConfiguration(It.IsAny<InputConfiguration>(), It.IsAny<InputConfiguration>()));
            storageProvider = storageProviderMock.Object;

            newConfiguration = new InputConfiguration
            {
                FileName = "test.json"
            };
            oldConfiguration = TestFactory.Copy(newConfiguration);
            oldConfiguration.FileName = "test2.json";
        }

        [Test]
        public void SavingNewConfiguration()
        {
            inputConfigProviderMock.Setup(i => i.Configs).Returns(new List<InputConfiguration>());
            cmd = new SaveInputConfiguration(storageProvider, appConfigProvider, inputConfigProvider, newConfiguration);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsTrue(result.Success);
            inputConfigProviderMock.Verify(i => i.Configs, Times.Once);
            inputConfigProviderMock.Verify(i => i.AddOrUpdate(It.IsAny<InputConfiguration>()),Times.Once);
            storageProviderMock.Verify(s => s.UpdateInputConfiguration(It.IsAny<InputConfiguration>(), It.IsAny<InputConfiguration>()), Times.Never);
            storageProviderMock.Verify(s => s.SaveInputConfiguration(It.IsAny<InputConfiguration>()), Times.Once);
        }

        [Test]
        public void UpdatingExistingConfiguration()
        {
            inputConfigProviderMock.Setup(i => i.Configs).Returns(new List<InputConfiguration> {oldConfiguration});
            cmd = new SaveInputConfiguration(storageProvider, appConfigProvider, inputConfigProvider, newConfiguration);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsTrue(result.Success);
            inputConfigProviderMock.Verify(i => i.Configs, Times.Once);
            inputConfigProviderMock.Verify(i => i.AddOrUpdate(It.IsAny<InputConfiguration>()), Times.AtLeastOnce);
            storageProviderMock.Verify(s => s.SaveInputConfiguration(It.IsAny<InputConfiguration>()), Times.Never);
            storageProviderMock.Verify(s => s.UpdateInputConfiguration(It.IsAny<InputConfiguration>(), It.IsAny<InputConfiguration>()), Times.Once);
        }

        [Test]
        public void CreatesViewFolder()
        {
            inputConfigProviderMock.Setup(i => i.Configs).Returns(new List<InputConfiguration>());
            newConfiguration.View = "view";
            cmd = new SaveInputConfiguration(storageProvider, appConfigProvider, inputConfigProvider, newConfiguration);
            cmd.Execute();
            storageProviderMock.Verify(v => v.CreateViewFolder(It.Is<string>(s => s == newConfiguration.View)));
        }
    }
}
