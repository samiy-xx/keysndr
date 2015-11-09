using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class SequenceKeyValuePair
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public int Value { get; set; }

        public SequenceKeyValuePair()
        {
        }

        public SequenceKeyValuePair(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
