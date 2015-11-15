using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class ActionController : ApiController
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IInputConfigProvider inputConfigProvider;

        public ActionController()
        {
            fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
        }

        public ActionController(IFileSystemProvider p, IAppConfigProvider a)
        {
            fileSystemProvider = p;
            appConfigProvider = a;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<IEnumerable<string>> GetAllConfigurations()
        {
            var cmd = new GetAllInputConfigurations(inputConfigProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<IEnumerable<string>> GetLegacyConfigurations()
        {
            var cmd = new GetLegacyInputConfigurations(inputConfigProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<IEnumerable<string>> GetViewConfigurations()
        {
            var cmd = new GetViewInputConfigurations(inputConfigProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<InputConfiguration> GetConfiguration(string name)
        {
            var cmd = new GetInputConfiguration(inputConfigProvider, name);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<InputConfiguration> GetNewConfiguration(int actionCount)
        {
            var cmd = new GenerateInputConfiguration(actionCount);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<InputAction> GetNewInputAction()
        {
            var cmd = new GenerateInputAction();
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpDelete]
        public ApiResult<Object> RemoveConfiguration(string name)
        {
            var cmd = new RemoveInputConfiguration(inputConfigProvider, fileSystemProvider, appConfigProvider, name);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> Execute(InputActionExecutionContainer actionContainer)
        {
            var cmd = new ExecuteInputAction(actionContainer);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> Save(InputConfiguration configuration)
        {
            var cmd = new SaveInputConfiguration(fileSystemProvider, appConfigProvider, inputConfigProvider, configuration);
            cmd.Execute();
            return cmd.Result;
        }
    }
}
