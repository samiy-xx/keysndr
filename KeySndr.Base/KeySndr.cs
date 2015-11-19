using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using KeySndr.Base.BeaconLib;
using KeySndr.Base.Domain;
using KeySndr.Base.Exceptions;
using KeySndr.Base.Providers;
using KeySndr.Common;
using KeySndr.Common.Providers;
using Microsoft.Owin.Hosting;
using Nowin;

namespace KeySndr.Base
{
    public class KeySndrApp
    {
        public const string WebFolderName = "Web";
        public const string ConfigurationsFolderName = "Configurations";
        public const string ScriptsFolderName = "Scripts";

        private IDisposable webServer;
        private AppConfig AppConfig;
        private Beacon beacon;
        private List<IProvider> providers;

        public KeySndrApp()
        {
            providers = new List<IProvider>();
        }

        public KeySndrApp(List<IProvider> p)
        {
            providers = p;
        }

        public void Run()
        {
            RegisterProvidersBeforeAppConfig();
            LoadAppConfig();
            RegisterProvidersAfterAppConfig();
            VerifyStorage();
            StartWebServer();

            TempEventNotifier.OnReloadRequested += OnReloadRequested;
            LoadInputConfigurations();
            LoadInputScripts();
        }

        private void OnReloadRequested(object sender, EventArgs eventArgs)
        {
            ReloadAll();
        }


        public void StopAll()
        {
            TempEventNotifier.OnReloadRequested -= OnReloadRequested;
            StopServer();
            beacon.Dispose();
            ObjectFactory.Destroy();
        }

        public void ReloadAll()
        {
            StopAll();
            Run();
        }

        private void VerifyStorage()
        {
            var fs = ObjectFactory.GetProvider<IFileSystemProvider>();
            var os = ObjectFactory.GetProvider<IStorageProvider>();
            try
            {
                fs.Verify();
                os.Verify();
                fs.SaveAppConfiguration();
            }
            catch (DataFolderDoesNotExistException e)
            {
                ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig.FirstTimeRunning = true;
            }
        }

        private void RegisterProvidersBeforeAppConfig()
        {
            foreach (var provider in providers)
            {
                ObjectFactory.AddProvider(provider);
            }
            ObjectFactory.AddProvider(new FileSystemProvider());
            ObjectFactory.AddProvider(new AppConfigProvider());
            ObjectFactory.AddProvider(new ScriptProvider());
            ObjectFactory.AddProvider(new InputConfigProvider());
            ObjectFactory.AddProvider(new SystemProvider());
        }

        private void RegisterProvidersAfterAppConfig()
        {
            if (AppConfig.FirstTimeRunning || !AppConfig.UseObjectStorage)
                ObjectFactory.AddProvider(new FileStorageProvider());

            else 
                ObjectFactory.AddProvider(new DbStorageProvider());    
        }

        private void LoadAppConfig()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading App Config");
            var fs = ObjectFactory.GetProvider<IFileSystemProvider>();
            var acp = ObjectFactory.GetProvider<IAppConfigProvider>();
            var config = fs.LoadAppConfiguration();
            acp.AppConfig = config ?? new AppConfig();
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
            var options = new StartOptions
            {
                ServerFactory = "Nowin",
                Port = 45889
            };

            var url = $"http://{AppConfig.LastIp}:{AppConfig.LastPort}";
            //Nowin.OwinServerFactory.
            webServer = WebApp.Start<Startup>(options);
        }

        private void StopServer()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Stopping web server");
            webServer.Dispose();
        }

        public async Task LoadInputConfigurations()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading input configurations");
            var inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
            var storageProvider = ObjectFactory.GetProvider<IStorageProvider>();
            inputConfigProvider.Clear();

            await Task.Run(() =>
            {
                foreach (var loadInputConfiguration in storageProvider.LoadInputConfigurations())
                {
                    inputConfigProvider.Add(loadInputConfiguration);
                }
            });
        }

        public async Task LoadInputScripts()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading Scripts");
            var sp = ObjectFactory.GetProvider<IScriptProvider>();
            var storageProvider = ObjectFactory.GetProvider<IStorageProvider>();
            sp.Clear();

            var fs = ObjectFactory.GetProvider<IFileSystemProvider>();
            await Task.Run(async () =>
            {
                foreach (var s in storageProvider.LoadInputScripts())
                {
                    sp.AddScript(s, true);
                    await s.RunTest();
                }
            });
        }

        
    }

    public static class TempEventNotifier
    {
        public static EventHandler<EventArgs> OnReloadRequested;

        public static void ReloadRequested()
        {
            var t = new Timer {Interval = 1000};
            t.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                t.Stop();
                OnReloadRequested?.Invoke(new object(), EventArgs.Empty);
            };
            t.Start();
        }
    }
}
