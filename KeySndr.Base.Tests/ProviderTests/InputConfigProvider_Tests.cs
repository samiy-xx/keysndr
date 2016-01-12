using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.ProviderTests
{
    [TestFixture]
    public class InputConfigProvider_Tests
    {
        private Mock<IStorageProvider> storageProviderMock;
        private IStorageProvider storageProvider;

        private IInputConfigProvider provider;

        [SetUp]
        public void Setup()
        {
            storageProviderMock = new Mock<IStorageProvider>();
            storageProviderMock.Setup(s => s.LoadInputConfigurations()).Returns(new List<InputConfiguration>
            {
                new InputConfiguration(),
                new InputConfiguration()
            });
            storageProvider = storageProviderMock.Object;
            provider = new InputConfigProvider(storageProvider);    
        }

        [Test]
        public void CreatedInstanceIsValid()
        {
            Assert.IsNotNull(provider);
            Assert.IsNotNull(provider.Configs);
            Assert.AreEqual(0, provider.Configs.Count());
        }

        [Test]
        public void Add_NewConfigCanBeAdded()
        {
            var c = new InputConfiguration();
            provider.Add(c);
            Assert.AreEqual(1, provider.Configs.Count());
            Assert.AreEqual(c, provider.Configs.First());
        }

        [Test]
        public void Add_DuplicateConfigsCanNotBeAdded()
        {
            var c = new InputConfiguration();
            provider.Add(c);
            provider.Add(c);
            Assert.AreEqual(1, provider.Configs.Count());
            var c2 = new InputConfiguration();
            provider.Add(c2);
            Assert.AreEqual(2, provider.Configs.Count());
        }

        [Test]
        public void AddOrUpdate_AddsWhenNewInstance()
        {
            var c = new InputConfiguration();
            provider.AddOrUpdate(c);
            Assert.AreEqual(1, provider.Configs.Count());
            Assert.AreEqual(c, provider.Configs.First());
        }

        [Test]
        public void AddOrUpdate_UpdatesWhenInstanceAddedAgain()
        {
            var c = new InputConfiguration {Name = "test"};
            provider.AddOrUpdate(c);
            Assert.AreEqual("test", provider.Configs.First().Name);
            var cc = c;
            cc.Name = "Bananas";
            provider.AddOrUpdate(cc);
            Assert.AreEqual(1, provider.Configs.Count());
            Assert.AreEqual("Bananas", provider.Configs.First().Name);
        }

        [Test]
        public void Remove_RemovesAddedInstance()
        {
            var c = new InputConfiguration();
            provider.AddOrUpdate(c);
            Assert.AreEqual(1, provider.Configs.Count());
            provider.Remove(c);
            Assert.AreEqual(0, provider.Configs.Count());
        }

        [Test]
        public void Remove_DoesntRemoveWhenInstanceNotInList()
        {
            var c = new InputConfiguration();
            provider.AddOrUpdate(c);
            Assert.AreEqual(1, provider.Configs.Count());
            var cc = new InputConfiguration();
            provider.Remove(cc);
            Assert.AreEqual(1, provider.Configs.Count());
        }

        [Test]
        public void FindConfigForName_FindsConfig()
        {
            var c = new InputConfiguration { Name = "test" };
            var cc = new InputConfiguration { Name = "test2" };
            provider.Add(c);
            provider.Add(cc);
            var p = provider.FindConfigForName("test2");
            Assert.AreEqual(cc, p);
            Assert.AreNotEqual(c, p);
        }

        [Test]
        public void Clear_Clears()
        {
            var c = new InputConfiguration { Name = "test" };
            var cc = new InputConfiguration { Name = "test2" };
            provider.Add(c);
            provider.Add(cc);
            Assert.AreEqual(2, provider.Configs.Count());
            provider.Clear();
            Assert.AreEqual(0, provider.Configs.Count());
        }

        [Test]
        public void Prepare_Prepares()
        {
            Assert.AreEqual(0, provider.Configs.Count());
            provider.Prepare().Wait(100);
            Assert.AreEqual(2, provider.Configs.Count());
        }

        [Test]
        public void Dispose_JustClearsTheList()
        {
            var c = new InputConfiguration();
            provider.Add(c);
            Assert.AreEqual(1, provider.Configs.Count());
            provider.Dispose();
            Assert.AreEqual(0, provider.Configs.Count());
        }
    }
}
