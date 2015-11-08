using System;
using System.Collections.Generic;
using System.Net;

namespace KeySndr.Base.BeaconLib
{
    class IpEndPointComparer : IComparer<IPEndPoint>
    {
        public static readonly IpEndPointComparer Instance = new IpEndPointComparer();

        public int Compare(IPEndPoint x, IPEndPoint y)
        {
            var c = String.Compare(x.Address.ToString(), y.Address.ToString(), StringComparison.Ordinal);
            if (c != 0) return c;
            return y.Port - x.Port;
        }
    }
}
