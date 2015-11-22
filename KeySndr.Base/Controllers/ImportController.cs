using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Controllers
{
    public class ImportController : ApiController
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly IAppConfigProvider appConfigProvider;

        public ImportController()
        {
            inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
            scriptProvider = ObjectFactory.GetProvider<IScriptProvider>();
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
        }

        public ImportController(IAppConfigProvider a, IInputConfigProvider i, IScriptProvider s, IStorageProvider t)
        {
            appConfigProvider = a;
            inputConfigProvider = i;
            scriptProvider = s;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public IHttpActionResult Upload(string configName)
        {
            var cmd = new ZipPackageForExport(inputConfigProvider, scriptProvider, appConfigProvider, configName);
            cmd.Execute();
            var stream = (MemoryStream)cmd.Result;
            return new FileResult(stream, Request);
        }
    }
}
