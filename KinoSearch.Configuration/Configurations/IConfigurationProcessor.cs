using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KinoSearch.Configuration.Configurations
{
    public interface IConfigurationProcessor
    {
        void Process(IServiceScope scope, IConfiguration configuration);
    }
}