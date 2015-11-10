using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Controllers
{
    public class ScriptsController : ApiController
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IScriptProvider scriptProvider;

        public ScriptsController()
        {
            fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
            scriptProvider = ObjectFactory.GetProvider<IScriptProvider>();
        }

        public ScriptsController(IFileSystemProvider p, IScriptProvider s)
        {
            fileSystemProvider = p;
            scriptProvider = s;
        }

        [HttpGet]
        public IEnumerable<string> GetAllScripts()
        {
            var cmd = new GetAllScripts(scriptProvider);
            cmd.Execute();
            return cmd.Result;
        }

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
