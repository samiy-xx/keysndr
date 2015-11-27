using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using KeySndr.Base.BeaconLib;
using KeySndr.Base.Domain;
using KeySndr.Base.Exceptions;
using KeySndr.Base.Providers;
using KeySndr.Common.Providers;
using Microsoft.Owin.Hosting;

namespace KeySndr.Base
{
    public class KeySndrApp
    {
        public const string WebFolderName = "Web";
        public const string ViewsFolderName = "Views";
        public const string ConfigurationsFolderName = "Configurations";
        public const string ScriptsFolderName = "Scripts";
        public const string MappingsFolderName = "Mappings";

        private IDisposable webServer;
        private AppConfig appConfig;
        private Beacon beacon;
        private readonly List<IProvider> providers;

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
            var os = ObjectFactory.GetProvider<IStorageProvider>();
            var fs = new FileSystemUtils();
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
            ObjectFactory.AddProvider(new AppConfigProvider());
            ObjectFactory.AddProvider(new ScriptProvider());
            ObjectFactory.AddProvider(new InputConfigProvider());
            ObjectFactory.AddProvider(new SystemProvider());
        }

        private void RegisterProvidersAfterAppConfig()
        {
            //if (appConfig.FirstTimeRunning || !appConfig.UseObjectStorage)
                ObjectFactory.AddProvider(new FileStorageProvider());
            //else 
            //    ObjectFactory.AddProvider(new DbStorageProvider());    
        }

        private void LoadAppConfig()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading App Config");
            var fs = new FileSystemUtils();
            var acp = ObjectFactory.GetProvider<IAppConfigProvider>();
            var config = fs.LoadAppConfiguration();
            acp.AppConfig = config ?? new AppConfig();
            appConfig = acp.AppConfig;
        }

        private void SetupBeacon(int port)
        {
            beacon = new Beacon("KeySndrServer", (ushort)port)
            {
                BeaconData = appConfig.BroadcastIdentifier
            };
            beacon.Start();
        }

        private void StartWebServer()
        {
            SetupBeacon(appConfig.LastPort);
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Starting web server");
            try
            {
                var options = new StartOptions
                {
                    ServerFactory = "Nowin",
                    Port = appConfig.LastPort
                };
                webServer = WebApp.Start<Startup>(options);
            }
            catch (TargetInvocationException e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
                if (!string.IsNullOrEmpty(e.InnerException?.Message))
                    ObjectFactory.GetProvider<ILoggingProvider>().Error(e.InnerException.Message, e.InnerException);
            }
        }

        private void StopServer()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Stopping web server");
            webServer?.Dispose();
        }

        public async Task LoadInputConfigurations()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading input configurations");
            var inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
            await inputConfigProvider.Prepare();
        }

        public async Task LoadInputScripts()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Loading Scripts");
            var sp = ObjectFactory.GetProvider<IScriptProvider>();
            await sp.Prepare();
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
