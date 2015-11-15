using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class InputActionExecutionContainer
    {
        [DataMember(Name = "processName")]
        public string ProcessName { get; set; }

        [DataMember(Name = "useForegroundWindow")]
        public bool UseForegroundWindow { get; set; }

        [DataMember(Name = "useDesktop")]
        public bool UseDesktop { get; set; }

        [DataMember(Name = "inputAction")]
        public InputAction InputAction { get; set; }

        public InputActionExecutionContainer()
        {
            ProcessName = string.Empty;
            UseDesktop = false;
            UseForegroundWindow = false;
        }
    }
}
