namespace KeySndr.Common
{
    public abstract class AppPreferences
    {
        public const string IpKey = "ip";
        public const string PortKey = "port";
        public const string FirstTimeKey = "first_time";
        public const string UseSoundsKey = "sound";
        public const string UseCacheKey = "cache";

        public string Ip { get; protected set; }
        public int Port { get; protected set; }
        public bool FirtsTimeRunning { get; protected set; }
        public bool UseSounds { get; protected set; }
        public bool UseCache { get; protected set; }
        public AppPreferences SetIp(string ip)
        {
            Ip = ip;
            return this;
        }

        public AppPreferences SetPort(int p)
        {
            Port = p;
            return this;
        }

        public AppPreferences SetFirstTimeRunning(bool b)
        {
            FirtsTimeRunning = b;
            return this;
        }

        public AppPreferences SetUseSounds(bool b)
        {
            UseSounds = b;
            return this;
        }

        public AppPreferences SetCache(bool b)
        {
            UseCache = b;
            return this;
        }
        public abstract void Write();
    }
}
