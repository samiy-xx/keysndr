using System;
using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetAllInputConfigurationItems : ICommand<ApiResult<IEnumerable<ConfigurationListItem>>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        public ApiResult<IEnumerable<ConfigurationListItem>> Result { get; private set; }

        public GetAllInputConfigurationItems(IInputConfigProvider p)
        {
            inputConfigProvider = p;
        }

        public void Execute()
        {
            try
            {
                Result = new ApiResult<IEnumerable<ConfigurationListItem>>
                {
                    Content = inputConfigProvider.Configs.Select(
                        i => new ConfigurationListItem {Name = i.Name, Type = i.HasView ? "View" : "Grid"}),
                    Success = true
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<IEnumerable<ConfigurationListItem>>
                {
                    Success = false,
                    Message = "Failed to get input configurations",
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
