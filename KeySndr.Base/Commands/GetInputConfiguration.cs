using System;
using KeySndr.Base.Providers;
using MacroToolServer.CommonPortable;

namespace KeySndr.Base.Commands
{
    public class GetInputConfiguration : ICommand<InputConfiguration>
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly string scriptName;

        public InputConfiguration Result { get; private set; }
        public bool Success { get; private set; }

        public GetInputConfiguration(IFileSystemProvider p, string name)
        {
            fileSystemProvider = p;
            scriptName = name;
        }

        public void Execute()
        {
            try
            {
                Result = fileSystemProvider.LoadInputConfigurationFromDisk(scriptName);
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
            }
        }
    }
}
