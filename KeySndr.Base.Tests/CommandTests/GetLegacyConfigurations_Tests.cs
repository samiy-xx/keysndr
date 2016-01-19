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
    public class GetLegacyConfigurations_Tests
    {
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private IInputConfigProvider inputConfigProvider;
        private GetLegacyInputConfigurations cmd;

        [SetUp]
        public void Setup()
        {
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(s => s.Configs).Returns(TestConfigs);
            inputConfigProvider = inputConfigProviderMock.Object;
            cmd = new GetLegacyInputConfigurations(inputConfigProvider);
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
            Assert.AreEqual(2, result.Content.Count());
            Assert.AreEqual("test1", result.Content.First());
            Assert.AreEqual("test3", result.Content.Last());
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
