using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using KeySndr.Base.Commands;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Controllers
{
    public class SystemController : ApiController
    {
        private readonly ISystemProvider systemProvider;

        public SystemController()
        {
            systemProvider = ObjectFactory.GetProvider<ISystemProvider>();
        }

        public SystemController(ISystemProvider p)
        {
            systemProvider = p;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<IEnumerable<string>> GetProcessNames()
        {
            var cmd = new GetProcessNames(systemProvider);
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<Object> Reload()
        {
            var cmd = new ReloadSystem();
            cmd.Execute();
            return cmd.Result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        public ApiResult<string> AssemblyVersion()
        {
            var cmd = new GetAssemblyVersion();
            cmd.Execute();
            return cmd.Result;
        }
    }
}
