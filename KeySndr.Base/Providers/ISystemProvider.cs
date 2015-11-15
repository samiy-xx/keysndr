using System.Collections.Generic;

namespace KeySndr.Base.Providers
{
    public interface ISystemProvider : IProvider
    {
        IEnumerable<string> ProcessNames();
    }
}
