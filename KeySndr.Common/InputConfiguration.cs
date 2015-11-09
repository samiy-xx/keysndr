using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class InputConfiguration
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        public string FileName { get; set; }

        [DataMember(Name = "view")]
        public string View { get; set; }

        [DataMember(Name = "actions")]
        public List<InputAction> Actions;

        public bool HasView => !string.IsNullOrEmpty(View);

        public InputConfiguration()
        {
            Name = string.Empty;
            View = string.Empty;
            FileName = string.Empty;
            Actions = new List<InputAction>();
        }

        public InputConfiguration(string name, List<InputAction> actions)
        {
            Name = name;
            FileName = string.Empty;
            Actions = actions;
            View = string.Empty;
        }

        public InputConfiguration(string name, string view, List<InputAction> actions)
            : this(name, actions)
        {
            View = view;
        }
    }
}
