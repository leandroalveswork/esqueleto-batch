using EsqueletoBatch.HiBatch;
using EsqueletoBatch.Job;
using EsqueletoBatch.Servico;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsqueletoBatch;
public class ConfiguradorServicos : IHiConfiguradorServicos
{
    public void ConfigurarServicos(IServiceCollection services, string environment, IConfigurationRoot configuration)
    {
        services.AddScoped<ISomarServico, SomarServico>();
        
        services.AddScoped<SalvarSomaJob>();

        services.AddSingleton(new HiCronJob(typeof(SalvarSomaJob), configuration.GetSection("Jobs:SalvarSomaJob:Cron").Value));
    }
}