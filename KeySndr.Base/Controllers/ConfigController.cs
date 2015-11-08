using System.Web.Http;
using KeySndr.Base.Commands;
using KeySndr.Base.Domain;

namespace KeySndr.Base.Controllers
{
    public class ConfigController : ApiController
    {
        public AppConfig GetAppConfig()
        {
            var cmd = new GetAppConfig();
            cmd.Execute();
            return cmd.Result;
        }

        public bool StoreAppConfig(AppConfig c)
        {
            var cmd = new StoreAppConfig(c);
            cmd.Execute();
            return cmd.Result == "Ok";
        }
    }
}
