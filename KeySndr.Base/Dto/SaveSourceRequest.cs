using KeySndr.Base.Domain;
using Newtonsoft.Json;

namespace KeySndr.Base.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SaveSourceRequest
    {
        [JsonProperty("script")]
        public InputScript Script { get; set; }

        [JsonProperty("sourceFileName")]
        public string SourceFileName { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
