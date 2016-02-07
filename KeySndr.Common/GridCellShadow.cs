using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class GridCellShadow
    {
        [DataMember(Name = "inset")]
        public bool Inset { get; set; }

        [DataMember(Name = "horizontal")]
        public int Horizontal { get; set; }

        [DataMember(Name = "vertical")]
        public int Vertical { get; set; }

        [DataMember(Name = "blur")]
        public int Blur { get; set; }

        [DataMember(Name = "spread")]
        public int Spread { get; set; }

        [DataMember(Name = "color")]
        public GridColor Color { get; set; }

        public GridCellShadow()
        {
            Inset = false;
            Horizontal = 0;
            Vertical = 0;
            Blur = 5;
            Spread = 5;
            Color = new GridColor();
        }
    }
}