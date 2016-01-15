using System.Threading.Tasks;
using Microsoft.Owin;

namespace KeySndr.Base.Middleware
{
    public class RedirectUrl : OwinMiddleware
    {
        private const string PortalRedirect = "/manage/portal.html";
        public RedirectUrl(OwinMiddleware next) : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            if (context.Request.Uri.PathAndQuery.Equals("/") || string.IsNullOrEmpty(context.Request.Uri.PathAndQuery))
            {
                context.Response.StatusCode = 301;
                context.Response.Headers.Set("Location", PortalRedirect);
            }
            await Next.Invoke(context);
        }
    }
}
