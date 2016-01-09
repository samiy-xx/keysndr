using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeySndr.Base.Providers;
using Newtonsoft.Json;

namespace KeySndr.Base.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InputScript
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sourceFileNames")]
        public List<string> SourceFileNames { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("inputs")]
        public KeyValuePair<string, object> Inputs { get; set; }
         
        public List<SourceFile> SourceFiles { get; set; }
        public bool HasSourceFiles => SourceFiles.Count > 0;
        public bool IsValid { get; set; }
        public List<string> Errors { get; private set; }

        public IScriptContext Context => ObjectFactory.GetProvider<IScriptProvider>().GetContext(this);

        public InputScript()
        {
            Id = Guid.NewGuid();
            SourceFiles = new List<SourceFile>();
            SourceFileNames = new List<string>();
            Errors = new List<string>();
            Inputs = new KeyValuePair<string, object>();
        }

        public async Task RunTest()
        {
            var ctx = Context;
            ctx.SetTestMode(true);

            await Task.Run(() =>
            {
                try
                {
                    ctx.Execute();
                    //ctx.Run();
                }
                catch (Exception e)
                {
                    ObjectFactory.GetProvider<ILoggingProvider>().Debug("RunTest for " + Name + " failed");
                    ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
                }
            });
            ctx.SetTestMode(false);
        }

        public override string ToString()
        {
            return Name;
        }

        protected bool Equals(InputScript other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InputScript) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
