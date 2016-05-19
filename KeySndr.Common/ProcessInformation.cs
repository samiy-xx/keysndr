using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class ProcessInformation
    {
        [DataMember(Name = "processName")]
        public string ProcessName { get; set; }

        [DataMember(Name = "hasWindow")]
        public bool HasWindow { get; set; }
    }
}
