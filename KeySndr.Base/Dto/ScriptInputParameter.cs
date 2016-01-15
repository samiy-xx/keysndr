using Newtonsoft.Json;

namespace KeySndr.Base.Dto
{
    public class ScriptInputParameter
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
