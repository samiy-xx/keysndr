using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Commands;
using KeySndr.Base.Dto;
using KeySndr.Base.Providers;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class SaveSource_Tests
    {
        private Mock<IScriptProvider> scriptProviderMock;
        private Mock<IStorageProvider> storageProviderMock;

        private IScriptProvider scriptProvider;
        private IStorageProvider storageProvider;

        private SaveSource cmd;

        [SetUp]
        public void Setup()
        {
            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProvider = scriptProviderMock.Object;

            storageProviderMock = new Mock<IStorageProvider>();
            storageProvider = storageProviderMock.Object;
        }
        [Test]
        public void Test()
        {
            var req = new SaveSourceRequest
            {

            };
            cmd = new SaveSource(scriptProvider, storageProvider, req);
            Assert.IsTrue(false);
        }
    }
}
