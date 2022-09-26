using Microsoft.Extensions.DependencyInjection;

namespace EsqueletoBatch.HiBatch;
public interface IHiConfiguradorLogger
{
    void ConfigurarLogger(IServiceCollection services);
}