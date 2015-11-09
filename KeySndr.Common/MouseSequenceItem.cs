using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class MouseSequenceItem
    {
        [DataMember(Name = "positionX")]
        public int X { get; set; }

        [DataMember(Name = "positionY")]
        public int Y { get; set; }

        [DataMember(Name = "button")]
        public int Button { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "time")]
        public int Time { get; set; }

        public MouseSequenceItem()
        {

        }

        public MouseSequenceItem(int posx, int posy, int type = 0, int button = 0, int time = 250)
        {
            Type = type;
            X = posx;
            Y = posy;
            Button = button;
            Time = time;
        }
    }
}
