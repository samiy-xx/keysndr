using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GetViewConfigurations_Tests
    {
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private IInputConfigProvider inputConfigProvider;
        private GetViewInputConfigurations cmd;

        [SetUp]
        public void Setup()
        {
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(s => s.Configs).Returns(TestConfigs);
            inputConfigProvider = inputConfigProviderMock.Object;
            cmd = new GetViewInputConfigurations(inputConfigProvider);
        }

        [Test]
        public void Executes()
        {
            cmd.Execute();
            var result = cmd.Result;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<IEnumerable<string>>(result.Content);
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual("test2", result.Content.First());
            inputConfigProviderMock.Verify(i => i.Configs, Times.Once);
        }

        private IEnumerable<InputConfiguration> TestConfigs()
        {
            return new List<InputConfiguration>
            {
                new InputConfiguration
                {
                    Name = "test1"
                },
                new InputConfiguration
                {
                    Name = "test2",
                    View = "test"
                },
                new InputConfiguration
                {
                    Name = "test3"
                }
            };
        }
    }
}
