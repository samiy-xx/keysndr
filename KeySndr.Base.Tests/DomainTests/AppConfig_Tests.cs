using KeySndr.Base.Domain;
using NUnit.Framework;

namespace KeySndr.Base.Tests.DomainTests
{
    [TestFixture]
    public class AppConfig_Tests
    {
        private AppConfig appConfig;

        [SetUp]
        public void Setup()
        {
            appConfig = new AppConfig();
        }

        [Test]
        public void AppConfig_IsInitialized()
        {
            Assert.IsNotNull(appConfig);
            Assert.IsNotNull(appConfig.BroadcastIdentifier);
            Assert.IsNotNull(appConfig.CheckUpdateOnStart);
            Assert.IsNotNull(appConfig.ConfigFolder);
            Assert.IsNotNull(appConfig.DataFolder);
            Assert.IsNotNull(appConfig.EnableKeyboardAndMouse);
            Assert.IsNotNull(appConfig.FirstTimeRunning);
            Assert.IsNotNull(appConfig.LastIp);
            Assert.IsNotNull(appConfig.LastPath);
            Assert.IsNotNull(appConfig.LastPort);
            Assert.IsNotNull(appConfig.ProcessNumber);
            Assert.IsNotNull(appConfig.ScriptsFolder);
            Assert.IsNotNull(appConfig.WebRoot);
            Assert.IsNotNull(appConfig.UpdateVersionCheckUrl);
        }
    }
}
