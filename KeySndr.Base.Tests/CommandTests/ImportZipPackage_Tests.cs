using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Providers;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class ImportZipPackage_Tests
    {
        private Mock<IAppConfigProvider> appConfigProviderMock;
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private Mock<IScriptProvider> scriptProviderMock;
        private Mock<IStorageProvider> storageProviderMock;

        private IAppConfigProvider appConfigProvider;
        private IInputConfigProvider inputConfigProvider;
        private IScriptProvider scriptProvider;
        private IStorageProvider storageProvider;

        [SetUp]
        public void Setup()
        {
            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProvider = appConfigProviderMock.Object;

            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProvider = inputConfigProviderMock.Object;

            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProvider = scriptProviderMock.Object;

            storageProviderMock = new Mock<IStorageProvider>();
            storageProvider = storageProviderMock.Object;
        }
    }
}
