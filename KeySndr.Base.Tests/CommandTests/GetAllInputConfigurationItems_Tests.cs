using System.Collections.Generic;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GetAllInputConfigurationItems_Tests
    {
        private Mock<IInputConfigProvider> inputConfigProviderMock;
        private IInputConfigProvider inputConfigProvider;
        private GetAllInputConfigurationItems cmd;

        [SetUp]
        public void Setup()
        {
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(s => s.Configs).Returns(TestConfigs);
            inputConfigProvider = inputConfigProviderMock.Object;
            cmd = new GetAllInputConfigurationItems(inputConfigProvider);
        }

        [Test]
        public void Executes()
        {
            cmd.Execute();
            var result = cmd.Result;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<IEnumerable<ConfigurationListItem>>(result.Content);
            inputConfigProviderMock.Verify(i => i.Configs, Times.Once);
        }

        private IEnumerable<InputConfiguration> TestConfigs()
        {
            return new List<InputConfiguration>
            {
                TestFactory.CreateTestInputConfiguration()
            };
        } 
    }
}
