using System.IO;
using KeySndr.Common;

namespace KeySndr.Base
{
    public interface IZipper
    {
        MemoryStream Zip(InputConfiguration inputConfig);
    }
}