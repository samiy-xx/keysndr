using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Controllers
{
    public class ExportController : ApiController
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;

        public ExportController()
        {
            inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
            scriptProvider = ObjectFactory.GetProvider<IScriptProvider>();
        }

        public ExportController(IAppConfigProvider a, IInputConfigProvider i, IScriptProvider s)
        {
            inputConfigProvider = i;
            scriptProvider = s;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public IHttpActionResult Download(string configName)
        {
            var cmd = new ZipPackageForExport(inputConfigProvider, scriptProvider, configName);
            cmd.Execute();
            var stream = (MemoryStream) cmd.Result;
            return new FileResult(stream, Request);
        }
    }
}
