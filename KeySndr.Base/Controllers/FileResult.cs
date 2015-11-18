using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace KeySndr.Base.Controllers
{
    public class FileResult : IHttpActionResult
    {
        private readonly MemoryStream value;
        readonly HttpRequestMessage request;

        public FileResult(MemoryStream value, HttpRequestMessage request)
        {
            this.value = value;
            this.request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(value),
                RequestMessage = request
            };
            return Task.FromResult(response);
        }
    }
}