using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class GridColor
    {
        [DataMember(Name = "red")]
        public int Red { get; set; }

        [DataMember(Name = "green")]
        public int Green { get; set; }

        [DataMember(Name = "blue")]
        public int Blue { get; set; }

        [DataMember(Name = "alpha")]
        public int Alpha { get; set; }

        public GridColor()
        {
            Red = 81;
            Green = 203;
            Blue = 238;
            Alpha = 1;
        }
    }
}