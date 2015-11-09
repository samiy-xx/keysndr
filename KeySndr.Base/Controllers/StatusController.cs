using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KeySndr.Base.Controllers
{
    public class StatusController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }
    }
}
