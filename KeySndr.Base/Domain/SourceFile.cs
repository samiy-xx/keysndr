using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KeySndr.Base.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SourceFile
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }


        public string Contents { get; set; }
        public bool IsValid { get; set; }
        public bool ParseOk { get; set; }
        public bool CanExecute { get; set; }
        public string Error { get; set; }

        public SourceFile()
        {
        }

        public SourceFile(string fileName, string contents)
        {
            FileName = fileName;
            Contents = contents;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
