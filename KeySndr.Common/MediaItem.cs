using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class MediaItem
    {
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        [DataMember(Name = "size")]
        public string Size { get; set; }

        [DataMember(Name = "repeat")]
        public string Repeat { get; set; }

        [DataMember(Name = "positionLeft")]
        public string PositionLeft { get; set; }

        [DataMember(Name = "positionTop")]
        public string PositionTop { get; set; }

        public MediaItem()
        {
            FileName = string.Empty;
            Size = "cover";
            Repeat = "no-repeat";
            PositionLeft = "center";
            PositionTop = "center";
        }
    }
}
