using KeySndr.Base.Domain;

namespace KeySndr.Base.Providers
{
    public interface IAppConfigProvider : IProvider
    {
        AppConfig AppConfig { get; set; }
    }
}
