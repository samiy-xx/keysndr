using System.IO;
using System.Net.Http.Headers;
using System.Web.Http;
using KeySndr.Base.Middleware;
using KeySndr.Base.Providers;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace KeySndr.Base
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var appConfig = ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig;

            appBuilder.Map(new PathString("/manage"), ConfigureManager);
            if (!string.IsNullOrEmpty(appConfig.WebRoot) && Directory.Exists(appConfig.WebRoot))
                ConfigureUserSpace(appBuilder);
            
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
            config.Routes.MapHttpRoute(
                "DefaultRedirect", // Route name
                string.Empty, // URL with parameters
                new { controller = "Home", action = "Redirect" });
            config.EnableCors();
            config.Formatters.FormUrlEncodedFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/zip"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            appBuilder.UseWebApi(config);
        }

        private void ConfigureUserSpace(IAppBuilder appBuilder)
        {
            var appConfig = ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig;
            var fileSystem = new PhysicalFileSystem(appConfig.WebRoot);
            /*appBuilder.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new[] { "index.html" },
                FileSystem = fileSystem
            });*/
            appBuilder.Use(typeof(RedirectUrl));
            appBuilder.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                RequestPath = new PathString(""),
                FileSystem = fileSystem
            });

            appBuilder.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString(""),
                FileSystem = fileSystem,
                ServeUnknownFileTypes = true
            });
        }

        private void ConfigureManager(IAppBuilder appBuilder)
        {
            var fileSystem = new PhysicalFileSystem("./Portal");
            appBuilder.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new[] {"index.html"},
                FileSystem = fileSystem
            });
            appBuilder.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString(""),
                FileSystem = fileSystem,
                ServeUnknownFileTypes = true,
            });
        }
    }
}
