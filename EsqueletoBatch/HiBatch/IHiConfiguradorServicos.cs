using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsqueletoBatch.HiBatch;
public interface IHiConfiguradorServicos
{
    void ConfigurarServicos(IServiceCollection services, string environment, IConfigurationRoot configuration);
}