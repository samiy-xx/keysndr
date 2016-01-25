using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class RemoveInputConfiguration_Tests
    {
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private Mock<IStorageProvider> storageProviderMock;
        private IInputConfigProvider inputConfigProvider;
        private IStorageProvider storageProvider;

        private InputConfiguration config1;
        private InputConfiguration notFoundConfig;
        private InputConfiguration throwConfig;
        private RemoveInputConfiguration cmd;

        [SetUp]
        public void Setup()
        {
            config1 = TestFactory.CreateTestInputConfiguration();
            config1.Name = "config1";

            notFoundConfig = TestFactory.CreateTestInputConfiguration();
            notFoundConfig.Name = "notFound";

            throwConfig = TestFactory.CreateTestInputConfiguration();
            throwConfig.Name = "throw";

            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(s => s.FindConfigForName(It.Is<string>(f => f.Equals(config1.Name)))).Returns(config1);
            inputConfigProviderMock.Setup(s => s.FindConfigForName(It.Is<string>(f => f.Equals(throwConfig.Name)))).Returns(throwConfig);
            inputConfigProviderMock.Setup(s => s.FindConfigForName(It.Is<string>(f => f.Equals(notFoundConfig.Name)))).Returns((InputConfiguration)null);
            inputConfigProvider = inputConfigProviderMock.Object;

            storageProviderMock = new Mock<IStorageProvider>();
            storageProviderMock.Setup(s => s.RemoveInputConfiguration(It.Is<InputConfiguration>(f => f.Equals(throwConfig)))).Throws<Exception>();
            storageProvider = storageProviderMock.Object;

            
        }

        [Test]
        public void ReturnsResultWhenNotFound()
        {
            cmd = new RemoveInputConfiguration(inputConfigProvider, storageProvider, notFoundConfig.Name);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsFalse(result.Success);
            inputConfigProviderMock.Verify(f => f.FindConfigForName(It.Is<string>(s => s.Equals(notFoundConfig.Name))), Times.Once);
        }

        [Test]
        public void ReturnsResultIfException()
        {
            cmd = new RemoveInputConfiguration(inputConfigProvider, storageProvider, throwConfig.Name);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsFalse(result.Success);
            inputConfigProviderMock.Verify(f => f.FindConfigForName(It.Is<string>(s => s.Equals(throwConfig.Name))), Times.Once);
            storageProviderMock.Verify(f => f.RemoveInputConfiguration(It.Is<InputConfiguration>(c => c.Equals(throwConfig))), Times.Once);
        }

        private IEnumerable<InputConfiguration> InputConfigs()
        {
            var c1 = TestFactory.CreateTestInputConfiguration();
            var c2 = TestFactory.CreateTestInputConfiguration();
            c1.Name = "c1";
            c2.Name = "c2";
            return new List<InputConfiguration>
            {
                c1,
                c2
            };
        } 
    }
}
