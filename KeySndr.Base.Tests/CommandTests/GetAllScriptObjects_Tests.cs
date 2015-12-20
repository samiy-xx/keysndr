using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class GetAllScriptObjects_Tests
    {
        private IScriptProvider scriptProvider;
        private Mock<IScriptProvider> scriptProviderMock;

        private GetAllScriptObjects cmd;
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
            cmd = new GetAllScriptObjects(scriptProvider);
        }

        [Test]
        public void Test()
        {
            cmd.Execute();
            scriptProviderMock.Verify(s => s.Scripts, Times.Once);

            Assert.IsNotNull(cmd);
            Assert.IsNotNull(cmd.Result);
            Assert.IsInstanceOf<ApiResult<IEnumerable<InputScript>>>(cmd.Result);
            Assert.IsInstanceOf<List<InputScript>>(cmd.Result.Content.ToList());
            Assert.AreEqual(2, cmd.Result.Content.Count());
        }
    }
}
