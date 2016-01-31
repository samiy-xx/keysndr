using System;
using System.Collections.Generic;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetMediaFileNames : ICommand<ApiResult<IEnumerable<string>>>
    {
        private readonly IStorageProvider storageProvider;
        private readonly InputConfiguration configuration;

        public ApiResult<IEnumerable<string>> Result { get; private set; }

        public GetMediaFileNames(IStorageProvider s, InputConfiguration c)
        {
            storageProvider = s;
            configuration = c;
        }

        public void Execute()
        {
            try
            {
                var fileNames = storageProvider.LoadMediaFileNames(configuration);
                Result = new ApiResult<IEnumerable<string>>
                {
                    Content = fileNames,
                    ErrorMessage = string.Empty,
                    Message = "OK",
                    Success = true
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<IEnumerable<string>>
                {
                    Content = new List<string>(),
                    ErrorMessage = e.Message,
                    Message = "Failed to get media files",
                    Success = false
                };
            }
        }
    }
}
