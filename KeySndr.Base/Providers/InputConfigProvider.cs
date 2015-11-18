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
            lock (configs)
            {
                if (!configs.Contains(config))
                    configs.Add(config);
            }
        }

        public void AddOrUpdate(InputConfiguration config)
        {
            lock (configs)
            {
                var index = configs.IndexOf(config);
                if (index > -1)
                {
                    configs[index] = config;
                    return;
                }
                configs.Add(config);
            }
        }
        public void Remove(InputConfiguration config)
        {
            lock (configs)
            {
                if (configs.Contains(config))
                    configs.Remove(config);
            } 
        }

        public InputConfiguration FindConfigForName(string name)
        {
            lock (configs)
            {
                return configs.FirstOrDefault(c => c.Name == name);
            }
        }

        public void Clear()
        {
            lock (configs)
            {
                configs.Clear();
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
