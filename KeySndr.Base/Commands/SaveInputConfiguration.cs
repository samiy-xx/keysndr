using System;
using KeySndr.Base.Providers;
using MacroToolServer.CommonPortable;

namespace KeySndr.Base.Commands
{
    public class SaveInputConfiguration : ICommand<String>
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly InputConfiguration configuration;

        public string Result { get; private set; }
        public bool Success { get; private set; }

        public SaveInputConfiguration(IFileSystemProvider fs, InputConfiguration c)
        {
            fileSystemProvider = fs;
            configuration = c;
        }

        public void Execute()
        {
            try
            {
                fileSystemProvider.SaveInputConfiguration(configuration);
                Result = "Ok";
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
                Result = e.Message;
            }
        }


    }
}
