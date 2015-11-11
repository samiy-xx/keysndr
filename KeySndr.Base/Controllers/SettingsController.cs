using System;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class SettingsController : ApiController
    {
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IFileSystemProvider fileSystemProvider;

        public SettingsController()
        {
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
        }

        public SettingsController(IFileSystemProvider p, IAppConfigProvider a)
        {
            fileSystemProvider = p;
            appConfigProvider = a;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<AppConfig> GetAppSettings()
        {
            var cmd = new GetAppSettings(appConfigProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public ApiResult<Object> StoreAppSettings(AppConfig c)
        {
            var cmd = new StoreAppConfig(fileSystemProvider, appConfigProvider, c);
            cmd.Execute();
            return cmd.Result;
        }
    }
}
