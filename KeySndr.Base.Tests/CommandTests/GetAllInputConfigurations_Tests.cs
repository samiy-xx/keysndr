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
    public class GetAllInputConfigurations_Tests
    {
        private IInputConfigProvider inputConfigProvider;
        private  Mock<IInputConfigProvider> inputConfigProviderMock;
        private GetAllInputConfigurations cmd;

        [SetUp]
        public void Setup()
        {
            var list = new List<InputConfiguration>
            {
                new InputConfiguration()
                {
                    Name = "name1"
                },
                new InputConfiguration()
                {
                    Name = "name2"
                }
            };
            inputConfigProviderMock = new Mock<IInputConfigProvider>();
            inputConfigProviderMock.Setup(c => c.Configs).Returns(list);
            inputConfigProvider = inputConfigProviderMock.Object;

            cmd = new GetAllInputConfigurations(inputConfigProvider);
        }

        [Test]
        public void Can_Get_All_Configuration_Names()
        {
            cmd.Execute();
            inputConfigProviderMock.Verify(c => c.Configs, Times.Once);
            Assert.IsNotNull(cmd.Result);
            Assert.IsNotNull(cmd.Result.Content);
            Assert.AreEqual(true, cmd.Result.Success);
            Assert.AreEqual(2, cmd.Result.Content.Count());
            Assert.AreEqual("name1", cmd.Result.Content.First());
        }
    }
}
