using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class ExportController : ApiController
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;

        public ExportController()
        {
            fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
            scriptProvider = ObjectFactory.GetProvider<IScriptProvider>();
        }

        public ExportController(IFileSystemProvider f, IAppConfigProvider a, IInputConfigProvider i, IScriptProvider s)
        {
            fileSystemProvider = f;
            appConfigProvider = a;
            inputConfigProvider = i;
            scriptProvider = s;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public IHttpActionResult Download(string configName)
        {
            var cmd = new ZipPackageForExport(fileSystemProvider, inputConfigProvider, appConfigProvider, scriptProvider, configName);
            cmd.Execute();
            var stream = (MemoryStream) cmd.Result;
            return new FileResult(stream, Request);
        }
    }

    public class FileResult : IHttpActionResult
    {
        private readonly MemoryStream value;
        readonly HttpRequestMessage request;

        public FileResult(MemoryStream value, HttpRequestMessage request)
        {
            this.value = value;
            this.request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(value),
                RequestMessage = request
            };
            return Task.FromResult(response);
        }
    }
}
