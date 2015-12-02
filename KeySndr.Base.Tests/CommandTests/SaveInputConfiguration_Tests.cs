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
        private FileStorageProvider storageProvider;
        private IFileSystemUtils fileSystemUtils;

        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private Mock<IAppConfigProvider> appConfigProviderMock;
        private Mock<FileStorageProvider> storageProviderMock;
        private Mock<IFileSystemUtils> fileSystemUtilsMock;

        private SaveInputConfiguration cmd;

        private InputConfiguration newConfiguration;
        private InputConfiguration oldConfiguration;

        [SetUp]
        public void Setup()
        {
            fileSystemUtilsMock = new Mock<IFileSystemUtils>();
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.IsAny<string>())).Returns(true);
            fileSystemUtils = fileSystemUtilsMock.Object;

            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProvider = inputConfigProviderMock.Object;

            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProvider = appConfigProviderMock.Object;

            storageProviderMock = new Mock<FileStorageProvider>(fileSystemUtils, appConfigProvider);
            storageProvider = storageProviderMock.Object;

            newConfiguration = new InputConfiguration
            {
                FileName = "test.json"
            };
        }

        [Test]
        public void SavingNewConfiguration()
        {
            inputConfigProviderMock.Setup(i => i.Configs).Returns(new List<InputConfiguration>());
            cmd = new SaveInputConfiguration(storageProvider, appConfigProvider, inputConfigProvider, newConfiguration);
            cmd.Execute();
            var result = cmd.Result;

            inputConfigProviderMock.Verify(i => i.Configs, Times.Once);
            storageProviderMock.Verify(s => s.UpdateInputConfiguration(It.IsAny<InputConfiguration>(), It.IsAny<InputConfiguration>()), Times.Never);
            storageProviderMock.Verify(s => s.SaveInputConfiguration(It.IsAny<InputConfiguration>()), Times.Once);

            //fileSystemUtilsMock.Verify(s => );
            //fileSystemUtilsMock.Verify(s => s.SaveObjectToDisk(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UpdatingExistingConfiguration()
        {

        }
    }
}
