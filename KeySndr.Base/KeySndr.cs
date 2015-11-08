using System;
using System.IO;
using System.Threading.Tasks;
using KeySndr.Base.BeaconLib;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using Microsoft.Owin.Hosting;

namespace KeySndr.Base
{
    public class KeySndrApp
    {
        private IDisposable webServer;
        private AppConfig AppConfig;
        private Beacon beacon;

        public KeySndrApp()
        {
            RegisterProviders();
        }

        public void Run()
        {
            LoadAppConfig();
            StartWebServer();

            LoadInputConfigurations();
            LoadInputScripts();
        }

        private void RegisterProviders()
        {
            ObjectFactory.AddProvider(new FileSystemProvider());
            ObjectFactory.AddProvider(new AppConfigProvider());
            ObjectFactory.AddProvider(new ScriptProvider());
            ObjectFactory.AddProvider(new InputConfigProvider());
        }

        private void LoadAppConfig()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading App Config");
            var fs = ObjectFactory.GetProvider<IFileSystemProvider>();
            var acp = ObjectFactory.GetProvider<IAppConfigProvider>();
            acp.AppConfig = fs.LoadAppConfiguration();
            AppConfig = acp.AppConfig;
        }

        private void SetupBeacon(int port)
        {
            beacon = new Beacon("KeySndrServer", (ushort)port)
            {
                BeaconData = AppConfig.BroadcastIdentifier
            };
            beacon.Start();
        }
        private void StartWebServer()
        {
            SetupBeacon(AppConfig.LastPort);
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Starting web server");
            var url = $"http://{AppConfig.LastIp}:{AppConfig.LastPort}";
            webServer = WebApp.Start<Startup>(url: url);
        }

        private void StopServer()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Stopping web server");
            webServer.Dispose();
        }

        public async Task LoadInputConfigurations()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading input configurations");
            var fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
            var inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();

            inputConfigProvider.Clear();

            await Task.Run(() =>
            {
                var count = 0;
                foreach (var f in fileSystemProvider.GetAllConfigurationFiles())
                {
                    count++;
                    var c = fileSystemProvider.LoadInputConfigurationFromDisk(f);
                    c.FileName = f;
                    inputConfigProvider.Add(c);
                }
            });
        }

        public async Task LoadInputScripts()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading Scripts");
            var sp = ObjectFactory.GetProvider<IScriptProvider>();
            sp.Clear();

            var fs = ObjectFactory.GetProvider<IFileSystemProvider>();
            await Task.Run(async () =>
            {
                var count = 0;
                foreach (var f in fs.GetAllScriptFiles())
                {

                    count++;
                    var script = fs.LoadObjectFromDiskAsJson<InputScript>(Path.Combine(AppConfig.ScriptsFolder, f));
                    script.FileName = f;
                    

                    foreach (var sourceFile in script.SourceFiles)
                    {
                        try
                        {
                            sourceFile.Contents = fs.LoadFileAsString(sourceFile.FileName);
                        }
                        catch (Exception e)
                        {
                            
                        }
                    }
                    sp.AddScript(script, true);
                    await script.RunTest();
                }
                
            });
        }
    }
}
