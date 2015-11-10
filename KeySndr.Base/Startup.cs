using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Owin.StaticFiles.DirectoryFormatters;
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
                defaults: new {id = RouteParameter.Optional}
                );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var fileSystem = new PhysicalFileSystem(appConfig.WebRoot);
          
            appBuilder.UseWebApi(config);
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
                ServeUnknownFileTypes = true
            });
        }

        private void ConfigureManager(IAppBuilder appBuilder)
        {
            var fileSystem = new PhysicalFileSystem("./Portal");
            //var fileSystem = new CustomEmbeddedFileSystem(typeof(Sender).Assembly, "KeySndr.Base");
            appBuilder.UseDefaultFiles(new DefaultFilesOptions {DefaultFileNames = new[] {"index.html"}});

            appBuilder.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                RequestPath = new PathString(""),
                Formatter = new HtmlDirectoryFormatter(),
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

    public class CustomEmbeddedFileSystem : IFileSystem
    {
        private readonly Assembly assembly;
        private readonly string baseNamespace;
        private readonly DateTime lastModified;

        public CustomEmbeddedFileSystem(string baseNamespace)
            : this(Assembly.GetCallingAssembly(), baseNamespace)
        {
        }

        public CustomEmbeddedFileSystem(Assembly a, string b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("assembly");
            }
            baseNamespace = string.IsNullOrEmpty(b) ? string.Empty : b + ".";
            assembly = a;
            lastModified = new FileInfo(assembly.Location).LastWriteTime;
        }

        public bool TryGetFileInfo(string subpath, out IFileInfo fileInfo)
        {
            // "/file.txt" expected.
            if (string.IsNullOrEmpty(subpath) || subpath[0] != '/')
            {
                fileInfo = null;
                return false;
            }

            var fileName = subpath.Substring(1);  // Drop the leading '/'
            var resourcePath = baseNamespace + fileName;
            if (assembly.GetManifestResourceInfo(resourcePath) == null)
            {
                fileInfo = null;
                return false;
            }
            fileInfo = new CustomFileInfo(assembly, resourcePath, fileName, lastModified);
            return true;
        }

        public bool TryGetDirectoryContents(string subpath, out IEnumerable<IFileInfo> contents)
        {
            // The file name is assumed to be the remainder of the resource name.

            // Non-hierarchal.
            if (!subpath.Equals("/"))
            {
                contents = null;
                return false;
            }

            IList<IFileInfo> entries = new List<IFileInfo>();

            // TODO: The list of resources in an assembly isn't going to change. Consider caching.
            var resources = assembly.GetManifestResourceNames();
           
            for (var i = 0; i < resources.Length; i++)
            {
                var resourceName = resources[i];
                if (resourceName.StartsWith(baseNamespace))
                {
                    entries.Add(new CustomFileInfo(
                        assembly, resourceName, resourceName.Substring(baseNamespace.Length), lastModified));
                }
            }

            contents = entries;
            return true;
        }

        private class CustomFileInfo : IFileInfo
        {
            public string Name { get; }
            public DateTime LastModified { get; }
            public string PhysicalPath => null;
            public bool IsDirectory => false;

            private readonly Assembly assembly;
            private readonly string resourcePath;
            private long? length;

            public CustomFileInfo(Assembly assembly, string resourcePath, string fileName, DateTime lastModified)
            {
                this.assembly = assembly;
                this.LastModified = lastModified;
                this.resourcePath = resourcePath;
                this.Name = fileName;
            }

            public Stream CreateReadStream()
            {
                var stream = assembly.GetManifestResourceStream(resourcePath);
                if (!length.HasValue)
                {
                    length = stream.Length;
                }
                return stream;
            }

            public long Length
            {
                get
                {
                    if (length.HasValue) return length.Value;
                    using (var stream = assembly.GetManifestResourceStream(resourcePath))
                    {
                        length = stream.Length;
                    }
                    return length.Value;
                }
            }   
        } 
    }
}
