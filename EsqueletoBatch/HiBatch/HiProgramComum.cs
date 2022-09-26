using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace EsqueletoBatch.HiBatch;
public class HiProgramComum
{
    public static bool EstaDebugando;

    public static async Task MainAsync(string[] args, IHiConfiguradorLogger hiConfiguradorLogger, IHiConfiguradorServicos configuradorServicos)
    {
        EstaDebugando = (Debugger.IsAttached || args.Contains("--console"));
        var environment = "";
        #if DEBUG
            environment = "Debug";
        #else
            environment = "Release";
        #endif

        if (EstaDebugando)
        {
            var hostBuilderDebg = new HostBuilder()
                .ConfigureServices((context, services) => {
                    EstaDebugando = true;
                    ConfigurarServicos(services, environment, hiConfiguradorLogger, configuradorServicos);
                });
            var caminhoProjeto = Directory.GetCurrentDirectory();
            await hostBuilderDebg.ConfigureAppConfiguration((hostContext, config) => 
            {
                EstaDebugando = true;
                config.AddJsonFile(ObterCaminhoAppSettings(), optional: false, reloadOnChange: true)
                    .AddJsonFile(ObterCaminhoAppSettingsEnvironment(environment), optional: true, reloadOnChange: true);
            }).RunConsoleAsync();
        }
        else
        {
            var hostBuilder = new HostBuilder()
                .ConfigureServices((context, services) => {
                    EstaDebugando = false;
                    ConfigurarServicos(services, environment, hiConfiguradorLogger, configuradorServicos);
                });

            await RunServiceAsync(
                hostBuilder.ConfigureAppConfiguration((hostContext, config) => 
                {
                    EstaDebugando = false;
                    config.AddJsonFile(ObterCaminhoAppSettings(), optional: false, reloadOnChange: true)
                        .AddJsonFile(ObterCaminhoAppSettingsEnvironment(environment), optional: true, reloadOnChange: true);
                })
            );
        }
    }

    public static string ObterCaminhoAppSettings()
    {
        if (EstaDebugando)
        {
            return Directory.GetCurrentDirectory() + "\\appsettings.json";
        }
        else
        {
            return "appsettings.json";
        }
    }

    public static string ObterCaminhoAppSettingsEnvironment(string environment)
    {
        if (EstaDebugando)
        {
            return Directory.GetCurrentDirectory() + "\\appsettings." + environment + ".json";
        }
        else
        {
            return "appsettings." + environment + ".json";
        }
    }

    public static async Task RunServiceAsync(IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
    {
        hostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, HiTempoDeVidaServico>());
        await hostBuilder.Build()
            .RunAsync(cancellationToken);
    }

    public static void ConfigurarServicos(IServiceCollection services, string environment, IHiConfiguradorLogger hiConfiguradorLogger, IHiConfiguradorServicos hiConfiguradorServicos)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(ObterCaminhoAppSettings(), optional: false, reloadOnChange: true)
            .AddJsonFile(ObterCaminhoAppSettingsEnvironment(environment), optional: true, reloadOnChange: true)
            .Build();

        services.AddScoped<IConfigurationRoot>(c => config);

        hiConfiguradorLogger.ConfigurarLogger(services);

        services.AddOptions();

        services.AddSingleton<IJobFactory, HiJobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<HiJobRunner>();
        services.AddHostedService<HiJobsHost>();

        hiConfiguradorServicos.ConfigurarServicos(services, environment, config);
    }

}