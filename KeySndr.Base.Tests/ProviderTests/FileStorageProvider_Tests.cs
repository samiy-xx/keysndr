﻿using System;
using KeySndr.Base.Providers;
using KeySndr.Common;
using Moq;
using NUnit.Framework;

namespace KeySndr.Base.Tests.ProviderTests
{
    [TestFixture]
    public class FileStorageProvider_Tests
    {
        private Mock<IFileSystemUtils> fileSystemUtilsMock;
        private Mock<IAppConfigProvider> appConfigProviderMock;
        private IFileSystemUtils fileSystemUtils;
        private IAppConfigProvider appConfigProvider;
        private IStorageProvider provider;

        [SetUp]
        public void Setup()
        {
            fileSystemUtilsMock = new Mock<IFileSystemUtils>();
            fileSystemUtils = fileSystemUtilsMock.Object;

            appConfigProviderMock = new Mock<IAppConfigProvider>();
            appConfigProviderMock.Setup(a => a.AppConfig).Returns(TestFactory.CreateTestAppConfig);
            appConfigProvider = appConfigProviderMock.Object;

            provider = new FileStorageProvider(fileSystemUtils, appConfigProvider);
        }

        [Test]
        public void Verify_CallsFileSystemUtilsVerify()
        {
            provider.Verify();
            fileSystemUtilsMock.Verify(f => f.Verify(), Times.Once);
        }

        [Test]
        public void SaveInputConfiguration_WithoutViewSavesToDisk()
        {
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.IsAny<string>())).Returns(true);

            var c = TestFactory.CreateTestInputConfiguration();
            provider.SaveInputConfiguration(c);
            fileSystemUtilsMock.Verify(f => f.DirectoryExists(It.IsAny<string>()), Times.Once);
            fileSystemUtilsMock.Verify(f => f.SaveObjectToDisk(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void SaveInputConfiguration_ThrowsIfConfigPathDoesNotExist()
        {
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.IsAny<string>())).Returns(false);
            var c = TestFactory.CreateTestInputConfiguration();
            c.View = "view";
            provider.SaveInputConfiguration(c);
        }

        [Test]
        public void CreateViewFolder_CreatesFolder()
        {
            const string path = "path";
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.IsAny<string>())).Returns(false);
            provider.CreateViewFolder(path);
            fileSystemUtilsMock.Verify(f => f.DirectoryExists(It.IsAny<string>()), Times.Once);
            fileSystemUtilsMock.Verify(f => f.CreateDirectory(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UpdateInputConfiguration_MovesFilesIfTheyDontExistAndSaves()
        {
            var newConfig = TestFactory.CreateTestInputConfiguration();
            newConfig.FileName = "Path1";
            var oldConfig = TestFactory.CreateTestInputConfiguration();
            oldConfig.FileName = "Path2";

            fileSystemUtilsMock.Setup(f => f.FileExists(It.Is<string>(s => s.EndsWith(newConfig.FileName)))).Returns(false);
            fileSystemUtilsMock.Setup(f => f.FileExists(It.Is<string>(s => s.EndsWith(oldConfig.FileName)))).Returns(true);
            provider.UpdateInputConfiguration(newConfig, oldConfig);
            fileSystemUtilsMock.Verify(f => f.MoveFile(It.Is<string>(s => s.EndsWith(oldConfig.FileName)), It.Is<string>(s => s.EndsWith(newConfig.FileName))), Times.Once);
            fileSystemUtilsMock.Verify(f => f.SaveObjectToDisk(It.Is<InputConfiguration>(i => i.Id == newConfig.Id), It.Is<string>(v => v.EndsWith(newConfig.FileName))));
        }

        [Test]
        public void UpdateInputConfiguration_DoesNotMovesFilesIfTheyDontExistAndSaves()
        {
            var newConfig = TestFactory.CreateTestInputConfiguration();
            newConfig.FileName = "Path1";
            var oldConfig = TestFactory.CreateTestInputConfiguration();
            oldConfig.FileName = "Path2";

            fileSystemUtilsMock.Setup(f => f.FileExists(It.Is<string>(s => s.EndsWith(newConfig.FileName)))).Returns(false);
            fileSystemUtilsMock.Setup(f => f.FileExists(It.Is<string>(s => s.EndsWith(oldConfig.FileName)))).Returns(false);
            provider.UpdateInputConfiguration(newConfig, oldConfig);
            fileSystemUtilsMock.Verify(f => f.MoveFile(It.Is<string>(s => s.EndsWith(oldConfig.FileName)), It.Is<string>(s => s.EndsWith(newConfig.FileName))), Times.Never);
            fileSystemUtilsMock.Verify(f => f.SaveObjectToDisk(It.Is<InputConfiguration>(i => i.Id == newConfig.Id), It.Is<string>(v => v.EndsWith(newConfig.FileName))));
        }
        /*[Test]
        public void SaveInputConfiguration_CreatesDirectoryForViewIfNotExist()
        {
            var c = TestFactory.CreateTestInputConfiguration();
            c.View = "view";
            var configPath = Path.Combine(appConfigProvider.AppConfig.ConfigFolder);
            var viewFolder = Path.Combine(appConfigProvider.AppConfig.ViewsRoot, c.View);
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.Is<string>(s => s == configPath))).Returns(true);
            fileSystemUtilsMock.Setup(f => f.DirectoryExists(It.Is<string>(s => s == viewFolder))).Returns(false);
            provider.SaveInputConfiguration(c);
            
            fileSystemUtilsMock.Verify(f => f.DirectoryExists(It.IsAny<string>()), Times.Exactly(2));
            fileSystemUtilsMock.Verify(f => f.SaveObjectToDisk(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            fileSystemUtilsMock.Verify(f => f.CreateDirectory(It.IsAny<string>()), Times.Once);
        }*/
    }


}
