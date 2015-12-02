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

        private SaveInputConfiguration cmd;

        private InputConfiguration newConfiguration;
        private InputConfiguration oldConfiguration;

        [SetUp]
        public void Setup()
        {
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProvider = inputConfigProviderMock.Object;
            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProvider = appConfigProviderMock.Object;
            storageProviderMock = new Mock<IStorageProvider>();
            storageProvider = storageProviderMock.Object;

            newConfiguration = new InputConfiguration();
            newConfiguration.FileName = "test.json";
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
        }

        [Test]
        public void UpdatingExistingConfiguration()
        {

        }
    }
}
