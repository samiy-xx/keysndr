using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Domain;

namespace KeySndr.Base.Commands
{
    public class StoreAppConfig : ICommand<String>
    {
        private readonly AppConfig config;
        public string Result { get; private set; }
        public bool Success { get; private set; }

        public StoreAppConfig(AppConfig c)
        {
            config = c;
        }

        public void Execute()
        {
            Success = true;
            Result = "Ok";
        }
    }
}
