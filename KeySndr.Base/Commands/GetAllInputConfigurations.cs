using System;
using System.Collections.Generic;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Commands
{
    public class GetAllInputConfigurations : ICommand<IEnumerable<string>>
    {
        private readonly IFileSystemProvider fileSystemProvider;
        public IEnumerable<string> Result { get; private set; }
        public bool Success { get; private set; }

        public GetAllInputConfigurations(IFileSystemProvider p)
        {
            fileSystemProvider = p;
        }

        public void Execute()
        {
            try
            {
                Result = fileSystemProvider.GetAllConfigurationFiles();
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
            }
        }
    }
}
