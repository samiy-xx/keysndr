using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class ConfigurationListItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
