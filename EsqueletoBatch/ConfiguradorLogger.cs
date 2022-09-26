using Microsoft.Extensions.DependencyInjection;

namespace EsqueletoBatch.HiBatch;
public class ConfiguradorLogger : IHiConfiguradorLogger
{
    public void ConfigurarLogger(IServiceCollection services)
    {
        services.AddSingleton<IHiLogger, HiConsoleLogger>();
    }
}