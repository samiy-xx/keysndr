using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class ActionController : ApiController
    {
        private readonly IStorageProvider storageProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IInputConfigProvider inputConfigProvider;

        public ActionController()
        {
            storageProvider = ObjectFactory.GetProvider<IStorageProvider>();
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            inputConfigProvider = ObjectFactory.GetProvider<IInputConfigProvider>();
        }

        public ActionController(IAppConfigProvider a, IInputConfigProvider i, IStorageProvider s)
        {
            appConfigProvider = a;
            inputConfigProvider = i;
            storageProvider = s;
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
        public ApiResult<IEnumerable<ConfigurationListItem>> GetAllConfigurationItems()
        {
            var cmd = new GetAllInputConfigurationItems(inputConfigProvider);
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
            var cmd = new RemoveInputConfiguration(inputConfigProvider, storageProvider, name);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public async Task<ApiResult<Object>> Execute(InputActionExecutionContainer actionContainer)
        {
            var cmd = new ExecuteInputAction(actionContainer);
            await cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> Save(InputConfiguration configuration)
        {
            var cmd = new SaveInputConfiguration(storageProvider, appConfigProvider, inputConfigProvider, configuration);
            cmd.Execute();
            return cmd.Result;
        }
    }
}
