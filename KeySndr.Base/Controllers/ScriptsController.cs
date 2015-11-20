using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class ScriptsController : ApiController
    {
        private readonly IStorageProvider storageProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IScriptProvider scriptProvider;

        public ScriptsController()
        {
            storageProvider = ObjectFactory.GetProvider<IStorageProvider>();
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            scriptProvider = ObjectFactory.GetProvider<IScriptProvider>();
        }

        public ScriptsController(IStorageProvider p, IAppConfigProvider a, IScriptProvider s)
        {
            storageProvider = p;
            appConfigProvider = a;
            scriptProvider = s;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<IEnumerable<string>> GetAllScripts()
        {
            var cmd = new GetAllScripts(scriptProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<IEnumerable<InputScript>> GetAllScriptObjects()
        {
            var cmd = new GetAllScriptObjects(scriptProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> Save(InputScript configuration)
        {
            var cmd = new SaveInputScript(storageProvider, appConfigProvider, scriptProvider, configuration);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> Remove(InputScript configuration)
        {
            var cmd = new RemoveInputScript(scriptProvider, storageProvider, configuration);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> Validate(InputScript configuration)
        {
            var cmd = new ValidateInputScript(scriptProvider, configuration);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public async Task<bool> ExecuteScript(string scriptName)
        {
            var ctx = scriptProvider.FindContextForName(scriptName);
            return await Task.Run(() =>
            {
                ctx.Execute();
                ctx.Run();
                return true;
            });
            
        }
    }
}
