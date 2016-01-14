using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Providers;
using NUnit.Framework;

namespace KeySndr.Base.Tests.ProviderTests
{
    [TestFixture]
    public class ScriptProvider_Tests
    {
        private IScriptProvider provider;

        [SetUp]
        public void Setup()
        {
            provider = new ScriptProvider();    
        }

        [Test]
        public void Initializes()
        {
            Assert.IsNotNull(provider.Scripts);
            Assert.IsNotNull(provider.Contexts);
        }

        [Test]
        public void AddScript_AddsOnlyOneInstance()
        {
            var s = TestFactory.CreateTestInputScript();
            provider.AddScript(s, false);
            provider.AddScript(s, false);
            Assert.AreEqual(1, provider.Scripts.Count());
            Assert.AreEqual(s, provider.Scripts.First());
        }

        [Test]
        public void AddScript_AddsAndCreatesContext()
        {
            var s = TestFactory.CreateTestInputScript();
            provider.AddScript(s, true);
            Assert.AreEqual(1, provider.Scripts.Count());
            Assert.AreEqual(s, provider.Scripts.First());
            Assert.AreEqual(1, provider.Contexts.Count());
            Assert.AreEqual(s, provider.Contexts.First().Script);
        }

        [Test]
        public void RemoveScript_RemovesScript()
        {
            var s1 = TestFactory.CreateTestInputScript();
            var s2 = TestFactory.CreateTestInputScript();
            provider.AddScript(s1, true);
            provider.AddScript(s2, true);
            Assert.AreEqual(s1, provider.Scripts.First());
            Assert.AreEqual(s2, provider.Scripts.Last());
            provider.RemoveScript(s1);
            Assert.AreEqual(s2, provider.Scripts.First());
        }

        [Test]
        public void RemoveScript_RemovesScriptAndContext()
        {
            var s1 = TestFactory.CreateTestInputScript();
            var s2 = TestFactory.CreateTestInputScript();
            provider.AddScript(s1, true);
            provider.AddScript(s2, true);
            Assert.AreEqual(s1, provider.Scripts.First());
            Assert.AreEqual(s2, provider.Scripts.Last());
            Assert.AreEqual(2, provider.Contexts.Count());
            provider.RemoveScript(s1);
            Assert.AreEqual(s2, provider.Scripts.First());
            Assert.AreEqual(1, provider.Contexts.Count());
            Assert.AreEqual(s2, provider.Contexts.First().Script);
        }

        [Test]
        public void Create_CreatesAndReturnsContext()
        {
            var s = TestFactory.CreateTestInputScript();
            var ctx = provider.Create(s);
            Assert.AreEqual(1, provider.Contexts.Count());
            Assert.AreEqual(ctx, provider.Contexts.First());
            Assert.AreEqual(ctx.Script, s);
        }

        [Test]
        public void FindContextForName_ReturnsContextForGivenSciptName()
        {
            var s1 = TestFactory.CreateTestInputScript();
            s1.Name = "s1";
            var s2 = TestFactory.CreateTestInputScript();
            s2.Name = "s2";

            provider.AddScript(s1, true);
            provider.AddScript(s2, true);
            var ctx = provider.FindContextForName(s2.Name);
            Assert.IsNotNull(ctx);
            Assert.AreEqual(ctx.Script, s2);
            Assert.AreNotEqual(ctx.Script, s1);
        }

        [Test]
        public void GetContext_ReturnsContextForGivenScript()
        {
            var s1 = TestFactory.CreateTestInputScript();
            var s2 = TestFactory.CreateTestInputScript();
            provider.AddScript(s1, true);
            provider.AddScript(s2, true);
            var ctx = provider.GetContext(s2);
            Assert.IsNotNull(ctx);
            Assert.AreEqual(ctx.Script, s2);
            Assert.AreNotEqual(ctx.Script, s1);
        }

        [Test]
        public void RemoveContext_RemovesContext()
        {
            var s1 = TestFactory.CreateTestInputScript();
            var s2 = TestFactory.CreateTestInputScript();
            provider.AddScript(s1, true);
            provider.AddScript(s2, true);
            //provider.Re;
        }
    }
}
