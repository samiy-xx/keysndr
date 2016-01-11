using Newtonsoft.Json;

namespace KeySndr.Base.Domain
{
    public class ScriptInputParameter
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
