using System;
using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Domain;
using KeySndr.Common;
using DBreeze;
using DBreeze.DataTypes;
using Newtonsoft.Json;

namespace KeySndr.Base.Providers
{
    public class DbStorageProvider : IStorageProvider
    {
        private DBreezeEngine engine = null;
        private bool isVerified;

        public DbStorageProvider()
        {
            isVerified = false;
        }

        public void Dispose()
        {
            engine?.Dispose();
            DBreeze.Utils.CustomSerializator.Serializator = JsonConvert.SerializeObject;
            DBreeze.Utils.CustomSerializator.Deserializator = JsonConvert.DeserializeObject;
        }

        public void Verify()
        {
            var acp = ObjectFactory.GetProvider<IAppConfigProvider>();
            if (engine != null)
                return;
            if (string.IsNullOrEmpty(acp.AppConfig.DataFolder))
                return;
            var config = new DBreezeConfiguration
            {
                Storage = DBreezeConfiguration.eStorage.DISK,
                DBreezeDataFolderName = acp.AppConfig.DataFolder
            };
            engine = new DBreezeEngine(config);
           
            isVerified = true;
        }

        public void SaveInputConfiguration(InputConfiguration c)
        {
            if (!isVerified)
                throw new Exception("Storage not verified");
            using (var transaction = engine.GetTransaction())
            {
                try
                {
                    transaction.Insert<string, DbMJSON<InputConfiguration>>(KeySndrApp.ConfigurationsFolderName, c.Name, c);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void SaveScript(InputScript s)
        {
            if (!isVerified)
                throw new Exception("Storage not verified");
            using (var transaction = engine.GetTransaction())
            {
                try
                {
                    transaction.Insert<string, DbMJSON<InputScript>>(KeySndrApp.ScriptsFolderName, s.Name, s);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    
                }
            }
        }

        public void RemoveInputConfiguration(InputConfiguration i)
        {
            if (!isVerified)
                throw new Exception("Storage not verified");
            using (var transaction = engine.GetTransaction())
            {
                try
                {
                    transaction.RemoveKey<string>(KeySndrApp.ConfigurationsFolderName, i.Name);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    
                }
            }
        }

        public void RemoveScript(InputScript s)
        {
            if (!isVerified)
                throw new Exception("Storage not verified");
            using (var transaction = engine.GetTransaction())
            {
                try
                {
                    transaction.RemoveKey<string>(KeySndrApp.ConfigurationsFolderName, s.Name);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                }
            }
        }

        public IEnumerable<InputConfiguration> LoadInputConfigurations()
        {
            if (!isVerified)
                throw new Exception("Storage not verified");
            IEnumerable<InputConfiguration> configurations;
            using (var transaction = engine.GetTransaction())
            {
                try
                {
                    configurations =
                            transaction.SelectForward<string, DbMJSON<InputConfiguration>>(KeySndrApp.ConfigurationsFolderName)
                           .Select(c => c.Value.Get).ToList();
                }
                catch (Exception e)
                {
                    configurations = new InputConfiguration[0];
                }
            }
            return configurations;
        }

        public IEnumerable<InputScript> LoadInputScripts()
        {
            if (!isVerified)
                throw new Exception("Storage not verified");
            IEnumerable<InputScript> scripts;
            using (var transaction = engine.GetTransaction())
            {
                try
                {
                    scripts =
                        transaction.SelectForward<string, DbMJSON<InputScript>>(KeySndrApp.ScriptsFolderName)
                            .Select(c => c.Value.Get)
                            .ToList();
                }
                catch (Exception e)
                {
                    scripts = new InputScript[0];
                }
            }
            return scripts;
        }
    }
}
