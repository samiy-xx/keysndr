using System.Collections.Generic;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public class SystemProvider : ISystemProvider
    {
        public IEnumerable<ProcessInformation> ProcessNames()
        {
            return WinUtils.GetProcessNames();
        }

        public void Dispose()
        {
            
        }
    }
}
