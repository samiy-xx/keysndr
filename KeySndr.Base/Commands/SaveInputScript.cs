using System;
using System.Linq;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class SaveInputScript : IAsyncCommand<ApiResult<Object>>
    {
        private readonly IStorageProvider storageProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly InputScript script;

        public ApiResult<Object> Result { get; private set; }

        public SaveInputScript(IStorageProvider fs, IScriptProvider s, InputScript c)
        {
            storageProvider = fs;
            scriptProvider = s;
            script = c;
        }

        public async Task Execute()
        {
            try
            {
                SaveToStorage();
                ReloadSources();
                AddOrUpdateScriptProvider();

                await RunTests();
                
                Result = new ApiResult<object>
                {
                    Content = "empty",
                    Success = true,
                    Message = "Ok"
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<object>
                {
                    Content = "empty",
                    Success = false,
                    Message = "Failed to save script " + script.Name,
                    ErrorMessage = e.Message
                };
            }
        }

        private void AddOrUpdateScriptProvider()
        {
            scriptProvider.AddOrUpdate(script, true);
        }

        private void SaveToStorage()
        {
            var existing = scriptProvider.Scripts.FirstOrDefault(c => c.Equals(script));
            if (existing == null)
            {
                storageProvider.SaveScript(script);
                return;
            }

            if (!existing.FileName.Equals(script.FileName))
                storageProvider.UpdateScript(script, existing);
            else
                storageProvider.SaveScript(script);
        }

        private void ReloadSources()
        {
            storageProvider.LoadAllSourceFiles(script);
        }
        
        private async Task RunTests()
        {
            await script.RunTest();
        }
    }
}
