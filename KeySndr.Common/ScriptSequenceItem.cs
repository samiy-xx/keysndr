using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class ScriptSequenceItem
    {
        [DataMember(Name = "scriptName")]
        public string ScriptName { get; set; }

        public ScriptSequenceItem()
            : base()
        {
            ScriptName = string.Empty;
        }

        public ScriptSequenceItem(string n)
            : this()
        {
            ScriptName = n;
        }
    }
}
