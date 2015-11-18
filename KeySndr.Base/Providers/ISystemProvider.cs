using System.Collections.Generic;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface ISystemProvider : IProvider
    {
        IEnumerable<string> ProcessNames();
    }
}
