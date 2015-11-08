using Newtonsoft.Json;

namespace KeySndr.Base.Domain
{
    public class ApiResult<T>
        where T : class
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("content")]
        public T Content { get; private set; }

        public ApiResult()
        {
        }
    }
}
