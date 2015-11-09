using System.Collections.Generic;
using System.Web.Http;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class ActionController : ApiController
    {
        private readonly IFileSystemProvider fileSystemProvider;

        public ActionController()
        {
            fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
        }

        public ActionController(IFileSystemProvider p)
        {
            fileSystemProvider = p;
        }

        [HttpGet]
        public IEnumerable<string> GetAllConfigurations()
        {
            var cmd = new GetAllInputConfigurations(fileSystemProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [HttpGet]
        public InputConfiguration GetConfiguration(string name)
        {
            var cmd = new GetInputConfiguration(fileSystemProvider, name);
            cmd.Execute();
            return cmd.Result;
        }

        [HttpPost]
        public void Execute(InputAction action)
        {
            var cmd = new ExecuteInputAction(action);
            cmd.Execute();
        }

        [HttpPost]
        public void Save(InputConfiguration configuration)
        {
            var cmd = new SaveInputConfiguration(fileSystemProvider, configuration);
            cmd.Execute();
        }
    }
}
