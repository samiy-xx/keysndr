using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class InputConfiguration
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        [DataMember(Name = "view")]
        public string View { get; set; }

        [DataMember(Name = "actions")]
        public List<InputAction> Actions;

        [DataMember(Name = "processName")]
        public string ProcessName { get; set; }

        [DataMember(Name = "useForegroundWindow")]
        public bool UseForegroundWindow { get; set; }

        [DataMember(Name = "useDesktopWindow")]
        public bool UseDesktopWindow { get; set; }

        public bool HasView => !string.IsNullOrEmpty(View);

        public InputConfiguration()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            View = string.Empty;
            FileName = string.Empty;
            ProcessName = string.Empty;
            UseForegroundWindow = false;
            UseDesktopWindow = false;
            Actions = new List<InputAction>();
        }

        public InputConfiguration(string name, List<InputAction> actions)
        {
            Id = Guid.NewGuid();
            Name = name;
            FileName = string.Empty;
            ProcessName = string.Empty;
            UseForegroundWindow = false;
            UseDesktopWindow = false;
            Actions = actions;
            View = string.Empty;
        }

        public InputConfiguration(string name, string view, List<InputAction> actions)
            : this(name, actions)
        {
            View = view;
        }

        public void AddAction(InputAction a)
        {
            Actions.Add(a);
        }

        protected bool Equals(InputConfiguration other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InputConfiguration) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
