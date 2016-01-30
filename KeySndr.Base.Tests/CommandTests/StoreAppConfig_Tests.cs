using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class StoreAppConfig_Tests
    {
        private Mock<IFileSystemUtils> fileSystemUtilsMock;
        private Mock<IAppConfigProvider> appConfigProviderMock;

        private IFileSystemUtils fileSystemUtils;
        private IAppConfigProvider appConfigProvider;

        private AppConfig appConfig;
        private StoreAppConfig cmd;

        [SetUp]
        public void Setup()
        {
            appConfig = TestFactory.CreateTestAppConfig();

            fileSystemUtilsMock = new Mock<IFileSystemUtils>();
            fileSystemUtils = fileSystemUtilsMock.Object;

            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProvider = appConfigProviderMock.Object;
        }

        [Test]
        public void AppConfigIsStored()
        {
            appConfig.FirstTimeRunning = true;
            cmd = new StoreAppConfig(appConfigProvider, fileSystemUtils, appConfig);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(appConfig.FirstTimeRunning);
            fileSystemUtilsMock.Verify(v => v.SaveAppConfiguration(), Times.Once);
            
        }
    }
}
