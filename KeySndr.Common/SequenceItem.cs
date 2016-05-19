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

        [DataMember(Name = "winmodifiers")]
        public List<SequenceKeyValuePair> WindowsModifiers { get; set; }

        //[DataMember(Name = "method")]
        //public string Method { get; set; }

        public SequenceItem()
            : this(0, "keyboard", null, new List<SequenceKeyValuePair>(), new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, int code, string key)
            : this(keepDown, "keyboard", new SequenceKeyValuePair(key, code), new List<SequenceKeyValuePair>(), new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, string method, int code, string key)
            : this(keepDown, method, new SequenceKeyValuePair(key, code), new List<SequenceKeyValuePair>(), new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, SequenceKeyValuePair entry)
            : this(keepDown, "keyboard", entry, new List<SequenceKeyValuePair>(), new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, string method, SequenceKeyValuePair entry)
            : this(keepDown, method, entry, new List<SequenceKeyValuePair>(), new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, SequenceKeyValuePair entry, List<SequenceKeyValuePair> modifiers)
            : this(keepDown, "keyboard", entry, modifiers, new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, string method, SequenceKeyValuePair entry, List<SequenceKeyValuePair> modifiers)
            : this(keepDown, method, entry, modifiers, new List<SequenceKeyValuePair>())
        {
        }

        public SequenceItem(int keepDown, string method, SequenceKeyValuePair entry, List<SequenceKeyValuePair> modifiers, List<SequenceKeyValuePair> winModifiers)
        {
            KeepDown = keepDown;
            //Method = method;
            Entry = entry;
            Modifiers = modifiers;
            WindowsModifiers = winModifiers;
        }

        public SequenceItem SetKeepDown(int i)
        {
            KeepDown = i;
            return this;
        }

        //public SequenceItem SetMethod(string m)
        //{
        //    Method = m;
        //    return this;
        //}

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
