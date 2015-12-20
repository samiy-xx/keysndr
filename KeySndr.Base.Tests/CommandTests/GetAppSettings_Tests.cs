using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GetAppSettings_Tests
    {
        private IAppConfigProvider provider;
        private Mock<IAppConfigProvider> provicerMock;
        private GetAppSettings cmd;

        [SetUp]
        public void Setup()
        {
            provicerMock = new Mock<IAppConfigProvider>();
            provicerMock.Setup(p => p.AppConfig).Returns(new AppConfig());
            provider = provicerMock.Object;
            cmd = new GetAppSettings(provider);
        }

        [Test]
        public void ReturnAppConfig()
        {
            cmd.Execute();
            var result = cmd.Result;
            provicerMock.Verify(p => p.AppConfig, Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ApiResult<AppConfig>>(result);
        }
    }
}
