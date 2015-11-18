using System.Collections.Generic;

namespace KeySndr.Base.Providers
{
    public class SystemProvider : ISystemProvider
    {
        public IEnumerable<string> ProcessNames()
        {
            return WinUtils.GetProcessNames();
        }

        public void Dispose()
        {
            
        }
    }
}
