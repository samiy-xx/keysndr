using System;
using System.Linq;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class SaveSource : IAsyncCommand<ApiResult<Object>>
    {
        public ApiResult<Object> Result { get; private set; }
        private readonly IScriptProvider scriptProvider;
        private readonly IStorageProvider storageProvider;
        private readonly SaveSourceRequest request;

        public SaveSource(IScriptProvider s, IStorageProvider t, SaveSourceRequest script)
        {
            scriptProvider = s;
            storageProvider = t;
            request = script;
        }

        public async Task Execute()
        {
            try
            {
                
                StoreSource();
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
                    Message = "Failed to save script source " + request.SourceFileName,
                    ErrorMessage = e.Message
                };
            }
        }

        private void StoreSource()
        {
            var storedScript = scriptProvider.Scripts.FirstOrDefault(s => s.Equals(request.Script));
            if (storedScript == null)
                return;

            var storedSource = storedScript.SourceFiles.FirstOrDefault(s => s.FileName.Equals(request.SourceFileName));
            if (storedSource == null)
                return;

            storedSource.Contents = request.Source;

            scriptProvider.AddOrUpdate(storedScript, true);
            storageProvider.SaveScriptSource(storedScript, storedSource.FileName, request.Source);
        }

        private async Task RunTests()
        {
            await request.Script.RunTest();
        }
    }
}
