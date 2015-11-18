using KeySndr.Base.Domain;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface IAppConfigProvider : IProvider
    {
        AppConfig AppConfig { get; set; }
    }
}
