using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class GridSettings
    {
        private const string DefaultMode = "any";

        [DataMember(Name = "rows")]
        public int Rows { get; set; }

        [DataMember(Name = "columns")]
        public int Columns { get; set; }

        [DataMember(Name = "mode")]
        public string Mode { get; set; }

        [DataMember(Name = "mediaItem")]
        public MediaItem MediaItem { get; set; }

        [DataMember(Name = "showImageBackground")]
        public bool ShowImageBackground { get; set; }

        [DataMember(Name = "showBorder")]
        public bool ShowBorder { get; set; }

        [DataMember(Name = "showShadow")]
        public bool ShowShadow { get; set; }

        [DataMember(Name = "cellSpacing")]
        public float CellSpacing { get; set; }

        [DataMember(Name = "cellShadow")]
        public GridCellShadow CellShadow { get; set; }

        [DataMember(Name = "cellBorder")]
        public GridCellBorder CellBorder { get; set; }

        [DataMember(Name = "color")]
        public GridColor Color { get; set; }

        public GridSettings()
        {
            Rows = 6;
            Columns = 5;
            Mode = DefaultMode;
            MediaItem = new MediaItem();
            ShowBorder = true;
            ShowShadow = true;
            ShowImageBackground = false;
            CellSpacing = 10;
            CellShadow = new GridCellShadow();
            CellBorder = new GridCellBorder();
            Color = new GridColor
            {
                Alpha = 1,
                Blue = 70,
                Green = 70,
                Red = 70
            };
        }
    }
}
