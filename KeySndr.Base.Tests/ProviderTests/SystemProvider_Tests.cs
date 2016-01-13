using System.Collections.Generic;
using KeySndr.Base.Providers;
using NUnit.Framework;

namespace KeySndr.Base.Tests.ProviderTests
{
    [TestFixture]
    public class SystemProvider_Tests
    {
        private ISystemProvider provider;

        [SetUp]
        public void Setup()
        {
            provider = new SystemProvider();
        }

        [Test]
        public void Initilizes()
        {
            Assert.IsNotNull(provider);
            var l = provider.ProcessNames();
            Assert.IsInstanceOf<IEnumerable<string>>(l);
        }
    }
}
