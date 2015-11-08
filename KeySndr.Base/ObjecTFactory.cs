using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySndr.Base
{
    public static class ObjectFactory
    {
        private static readonly List<IProvider> Providers = new List<IProvider>();

        public static void AddProvider(IProvider provider)
        {
            if (Providers.Contains(provider))
                return;

            Providers.Add(provider);
        }

        public static T GetProvider<T>()
            where T : IProvider
        {
            return (T)Providers.FirstOrDefault(p => p.GetType().GetInterfaces().Any(i => i == typeof(T)));
        }
    }
}
