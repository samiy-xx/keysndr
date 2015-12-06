using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;
using NUnit.Framework;
using Moq;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GetAllScripts_Tests
    {
        private IScriptProvider scriptProvider;
        private Mock<IScriptProvider> scriptProviderMock;

        private GetAllScripts cmd;
        [SetUp]
        public void Setup()
        {
            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProviderMock.Setup(s => s.Scripts).Returns(new List<InputScript>
            {
                new InputScript { FileName = "test.js"},
                new InputScript {FileName = "test2.js"}
            });
            scriptProvider = scriptProviderMock.Object;
            cmd = new GetAllScripts(scriptProvider);
        }

        [Test]
        public void Test()
        {
            cmd.Execute();
            scriptProviderMock.Verify(s => s.Scripts, Times.Once);

            Assert.IsNotNull(cmd);
            Assert.IsNotNull(cmd.Result);
            Assert.IsInstanceOf<ApiResult<IEnumerable<string>>>(cmd.Result);
            Assert.IsInstanceOf<List<string>>(cmd.Result.Content.ToList());
            Assert.AreEqual(2, cmd.Result.Content.Count());
        }
    }
}
