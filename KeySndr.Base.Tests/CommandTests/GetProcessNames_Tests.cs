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
    public class GetProcessNames_Tests
    {
        private Mock<ISystemProvider> systemProviderMock;
        private ISystemProvider systemProvider;
        private GetProcessNames cmd;

        [SetUp]
        public void Setup()
        {
            systemProviderMock = new Mock<ISystemProvider>();
            systemProviderMock.Setup(s => s.ProcessNames()).Returns(
                new List<ProcessInformation>
                {
                    new ProcessInformation {HasWindow = false, ProcessName = "a"},
                    new ProcessInformation {HasWindow = false, ProcessName = "b"}
                });
            systemProvider = systemProviderMock.Object;
        }

        [Test]
        public void Test()
        {
            cmd = new GetProcessNames(systemProvider);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(2, result.Content.Count());
        }
    }
}
