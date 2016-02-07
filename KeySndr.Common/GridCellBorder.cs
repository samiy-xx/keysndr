using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class GridCellBorder
    {
        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "style")]
        public string Style { get; set; }

        [DataMember(Name = "color")]
        public GridColor Color { get; set; }

        public GridCellBorder()
        {
            Size = 1;
            Style = "solid";
            Color = new GridColor();
        }
    }
}