using System;
using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Domain;
using KeySndr.Common;
using DBreeze;

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
        }

        public void Verify()
        {
            var acp = ObjectFactory.GetProvider<IAppConfigProvider>();
            if (engine != null)
                return;
            if (string.IsNullOrEmpty(acp.AppConfig.DataFolder))
                return;
            engine = new DBreezeEngine(acp.AppConfig.DataFolder);
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
                    transaction.Insert<string, InputConfiguration>(KeySndrApp.ConfigurationsFolderName, c.Name, c);
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
                    transaction.Insert<string, InputScript>(KeySndrApp.ScriptsFolderName, s.Name, s);
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
                        transaction.SelectForward<string, InputConfiguration>(KeySndrApp.ScriptsFolderName)
                            .Select(c => c.Value)
                            .ToList();
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
                        transaction.SelectForward<string, InputScript>(KeySndrApp.ScriptsFolderName)
                            .Select(c => c.Value)
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
