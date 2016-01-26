using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class GridSettings
    {
        [DataMember(Name = "rows")]
        public int Rows { get; set; }

        [DataMember(Name = "columns")]
        public int Columns { get; set; }

        public GridSettings()
        {
            Rows = 6;
            Columns = 5;
        }
    }
}
