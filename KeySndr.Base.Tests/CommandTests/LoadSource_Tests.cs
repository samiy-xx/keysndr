using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Dto;
using KeySndr.Base.Providers;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.CommandTests
{
    [TestFixture]
    public class LoadSource_Tests
    {
        private Mock<IScriptProvider> scriptProviderMock;
        private IScriptProvider scriptProvider;
        private GetSourceRequest reqDto;
        private LoadSource cmd;
        private InputScript toMatch;

        [SetUp]
        public void Setup()
        {
            scriptProviderMock = new Mock<IScriptProvider>();
            scriptProviderMock.Setup(s => s.Scripts).Returns(Scripts);
            scriptProvider = scriptProviderMock.Object;

            toMatch = TestFactory.CreateTestInputScript();
        }

        [Test]
        public void LoadSource_FindsScript()
        {
            reqDto = new GetSourceRequest
            {
                Script = toMatch,
                SourceFileName = "script.js"
            };
            cmd = new LoadSource(scriptProvider, reqDto);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(toMatch.SourceFiles.First().Contents, result.Content);

            scriptProviderMock.Verify(s => s.Scripts, Times.Once);
        }

        [Test]
        public void LoadSource_DoesNotFindScriptSource()
        {
            reqDto = new GetSourceRequest
            {
                Script = toMatch,
                SourceFileName = "nonexisting.js"
            };
            cmd = new LoadSource(scriptProvider, reqDto);
            cmd.Execute();
            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("empty", result.Content);

            scriptProviderMock.Verify(s => s.Scripts, Times.Once);
        }

        private IEnumerable<InputScript> Scripts()
        {
            var script1 = TestFactory.CreateTestInputScript();
            var script2 = TestFactory.CreateTestInputScript();
            

            return new List<InputScript>
            {
                script1,
                script2,
                toMatch
            };
        } 
    }
}
