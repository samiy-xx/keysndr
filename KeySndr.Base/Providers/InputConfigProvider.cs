using System.Collections.Generic;
using System.Linq;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public class InputConfigProvider : IInputConfigProvider
    {
        private readonly List<InputConfiguration> configs;
        public IEnumerable<InputConfiguration> Configs => configs;

        public InputConfigProvider()
        {
            configs = new List<InputConfiguration>();
        }

        public void Add(InputConfiguration config)
        {
            if (!configs.Contains(config))
                configs.Add(config);
        }

        public void Remove(InputConfiguration config)
        {
            if (configs.Contains(config))
                configs.Remove(config);
        }
        /*public async Task Store(InputConfiguration config)
        {
            var existing = FindConfigForName(config.Name);
            if (existing != null)
            {
                var index = configs.IndexOf(existing);
                if (index > -1)
                    configs[index] = config;
            }

            var fs = ObjectFactory.GetProvider<IFileSystemProvider>();
            await fs.SaveInputConfiguration(config);
        }*/

        public InputConfiguration FindConfigForName(string name)
        {
            return configs.FirstOrDefault(c => c.Name == name);
        }

        public void Clear()
        {
            configs.Clear();
        }
    }
}
