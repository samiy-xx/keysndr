using System.Collections.Generic;
using KeySndr.Common;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface ISystemProvider : IProvider
    {
        IEnumerable<ProcessInformation> ProcessNames();
    }
}
