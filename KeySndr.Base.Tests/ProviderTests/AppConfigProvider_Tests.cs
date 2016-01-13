using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using NUnit.Framework;

namespace KeySndr.Base.Tests.ProviderTests
{
    [TestFixture]
    public class AppConfigProvider_Tests
    {
        private IAppConfigProvider provider;

        [SetUp]
        public void Setup()
        { 
            provider = new AppConfigProvider();  
        }

        [Test]
        public void Initializes()
        {
            Assert.IsNotNull(provider);
            Assert.IsNotNull(provider.AppConfig);
            Assert.IsInstanceOf<AppConfig>(provider.AppConfig);
        }
    }
}
