using System;
using System.Collections.Generic;
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
    public class GetInputConfiguration_Tests
    {
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private IInputConfigProvider inputConfigProvider;
        private GetInputConfiguration cmd;
        private const string configNameFound = "test";
        private const string configNameNotFound = "test2";

        [SetUp]
        public void Setup()
        {
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(i => i.FindConfigForName(It.Is<string>(x => x.Equals(configNameFound))))
                .Returns(new InputConfiguration(configNameFound, new List<InputAction>()));
            inputConfigProvider = inputConfigProviderMock.Object;
        }

        [Test]
        public void FindsConfigIfItsPresent()
        {
            cmd = new GetInputConfiguration(inputConfigProvider, configNameFound);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Success);
            Assert.AreEqual(true, result.Success);
            Assert.IsInstanceOf<InputConfiguration>(result.Content);
            inputConfigProviderMock.Verify(i => i.FindConfigForName(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void DoesNotFindConfigIfItsNotPresent()
        {
            cmd = new GetInputConfiguration(inputConfigProvider, configNameNotFound);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Success);
            Assert.AreEqual(false, result.Success);
            inputConfigProviderMock.Verify(i => i.FindConfigForName(It.IsAny<string>()), Times.Once);
        }
    }
}
