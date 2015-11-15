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
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sourceFiles")]
        public List<SourceFile> SourceFiles { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        public bool HasSourceFiles => SourceFiles.Count > 0;
        public bool IsValid { get; set; }
        public List<string> Errors { get; private set; }

        public IScriptContext Context => ObjectFactory.GetProvider<IScriptProvider>().GetContext(this);

        public InputScript()
        {
            SourceFiles = new List<SourceFile>();
            Errors = new List<string>();
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
                    ctx.Run();
                }
                catch (Exception e)
                {

                }
            });
            ctx.SetTestMode(false);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
