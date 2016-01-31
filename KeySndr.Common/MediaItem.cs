using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KeySndr.Common
{
    [DataContract]
    public class MediaItem
    {
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        public MediaItem()
        {
            FileName = string.Empty;
        }
    }
}
