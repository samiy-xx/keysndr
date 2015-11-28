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
            FileName = string.Empty;
            Contents = string.Empty;
            IsValid = false;
            ParseOk = false;
            CanExecute = false;
            Error = string.Empty;
        }

        public SourceFile(string fileName, string contents)
        {
            IsValid = false;
            ParseOk = false;
            CanExecute = false;
            Error = string.Empty;
            FileName = fileName;
            Contents = contents;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
