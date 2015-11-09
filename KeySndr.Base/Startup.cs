using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
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
            appBuilder.Map(new PathString("/manage"), ConfigureManager);
            
            var appConfig = ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig;
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var fileSystem = new PhysicalFileSystem(appConfig.WebRoot);
            
            appBuilder.UseWebApi(config);
            appBuilder.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                RequestPath = new PathString("/browse"),
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
            //var fileSystem = new PhysicalFileSystem("./Web");
            var fileSystem = new EmbeddedResourceFileSystem(typeof(Sender).Assembly, "KeySndr.Base");
            appBuilder.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new[] { "index.html" } });

            appBuilder.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                RequestPath = new PathString(""),
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
