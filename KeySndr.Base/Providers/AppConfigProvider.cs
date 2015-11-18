using KeySndr.Base.Domain;

namespace KeySndr.Base.Providers
{
    public class AppConfigProvider : IAppConfigProvider
    {
        public AppConfig AppConfig { get; set; }

        public AppConfigProvider()
        {
            AppConfig = new AppConfig();
        }

        public AppConfigProvider(AppConfig c)
        {
            AppConfig = c;
        }

        public void Dispose()
        {
            
        }
    }
}
