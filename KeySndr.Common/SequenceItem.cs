using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class SequenceItem
    {
        [DataMember(Name = "keepdown")]
        public int KeepDown { get; set; }

        [DataMember(Name = "entry")]
        public SequenceKeyValuePair Entry { get; set; }

        [DataMember(Name = "modifiers")]
        public List<SequenceKeyValuePair> Modifiers { get; set; }

        public SequenceItem()
            : base()
        {
            Modifiers = new List<SequenceKeyValuePair>();
        }

        public SequenceItem(int keepDown, int code, string key)
            : this()
        {
            KeepDown = keepDown;
            Entry = new SequenceKeyValuePair(key, code);
        }

        public SequenceItem(int keepDown, SequenceKeyValuePair entry)
            : this()
        {
            KeepDown = keepDown;
            Entry = entry;
        }

        public SequenceItem(int keepDown, SequenceKeyValuePair entry, List<SequenceKeyValuePair> modifiers)
            : base()
        {
            KeepDown = keepDown;
            Entry = entry;
            Modifiers = modifiers;
        }

        public SequenceItem SetKeepDown(int i)
        {
            KeepDown = i;
            return this;
        }

        public SequenceItem SetEntry(SequenceKeyValuePair kv)
        {
            Entry = kv;
            return this;
        }

        public SequenceItem SetModifiers(List<SequenceKeyValuePair> m)
        {
            Modifiers = m;
            return this;
        }
    }
}
