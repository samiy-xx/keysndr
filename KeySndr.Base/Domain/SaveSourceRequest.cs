using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KeySndr.Base.Domain
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
