using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class ZipPackageForExport_Tests
    {
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private Mock<IScriptProvider> scriptProviderMock;
        private Mock<IAppConfigProvider> appConfigProviderMock;
        private Mock<IZipper> zipperMock;
         
        private IInputConfigProvider inputConfigProvider;
        private IScriptProvider scriptProvider;
        private IAppConfigProvider appConfigProvider;
        private IZipper zipper;

        private ZipPackageForExport cmd;
        private string existingConfigName;
        private string nonExistingConfigName;

        [SetUp]
        public void Setup()
        {
            existingConfigName = "existing";
            nonExistingConfigName = "nonexisting";
            
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(s => s.FindConfigForName(It.Is<string>(v => v.Equals(existingConfigName)))).Returns(TestFactory.CreateTestInputConfiguration);
            inputConfigProviderMock.Setup(s => s.FindConfigForName(It.Is<string>(v => v.Equals(nonExistingConfigName)))).Returns((InputConfiguration)null);
            inputConfigProvider = inputConfigProviderMock.Object;

            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProvider = scriptProviderMock.Object;

            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProvider = appConfigProviderMock.Object;

            zipperMock = new Mock<IZipper>();
            
            zipper = zipperMock.Object;
        }

        [Test]
        public void ZipsWithExistingConfig()
        {
            cmd = new ZipPackageForExport(inputConfigProvider, scriptProvider, appConfigProvider, zipper, existingConfigName);
            cmd.Execute();

            inputConfigProviderMock.Verify(v => v.FindConfigForName(It.Is<string>(s => s.Equals(existingConfigName))), Times.Once);
            zipperMock.Verify(v => v.Zip(It.IsAny<InputConfiguration>()), Times.Once);
        }

        [Test]
        public void DoesNotZipWithNonExistingConfig()
        {
            cmd = new ZipPackageForExport(inputConfigProvider, scriptProvider, appConfigProvider, zipper, nonExistingConfigName);
            cmd.Execute();

            inputConfigProviderMock.Verify(v => v.FindConfigForName(It.Is<string>(s => s.Equals(nonExistingConfigName))), Times.Once);
            zipperMock.Verify(v => v.Zip(It.IsAny<InputConfiguration>()), Times.Never);
        }
    }
}
