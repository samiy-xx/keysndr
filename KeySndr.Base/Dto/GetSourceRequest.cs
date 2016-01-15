using KeySndr.Base.Domain;
using Newtonsoft.Json;

namespace KeySndr.Base.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GetSourceRequest
    {
        [JsonProperty("script")]
        public InputScript Script { get; set; }

        [JsonProperty("sourceFileName")]
        public string SourceFileName { get; set; }
    }
}
