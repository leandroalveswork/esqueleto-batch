using EsqueletoBatch.HiBatch;
using EsqueletoBatch.Servico;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace EsqueletoBatch.Job;
public class SalvarSomaJob : HiJobBase, IJob
{
    private readonly ISomarServico _somarServico;
    private readonly IHiLogger _hiLogger;
    private readonly IConfigurationRoot _config;
    public SalvarSomaJob(ISomarServico somarServico, IHiLogger hiLogger, IConfigurationRoot config) : base()
    {
        _somarServico = somarServico;
        _hiLogger = hiLogger;
        _config = config;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _hiLogger.ImprimirLinha("<> [ EXECUÇÃO: Exportação da Soma ] </>");
            _hiLogger.ImprimirLinha("    [ Guid ] " + _guid);
            _hiLogger.ImprimirLinha("    [ Hora do Início ] " + DateTime.Now);
            _hiLogger.ImprimirLinha("    [ Início da Execução... ]");

            var n1 = int.Parse(_config.GetSection("Jobs:SalvarSomaJob:Params:N1").Value);
            var n2 = int.Parse(_config.GetSection("Jobs:SalvarSomaJob:Params:N2").Value);
            var caminhoCompletoArqSalvar = _config.GetSection("Jobs:SalvarSomaJob:Params:CaminhoCompletoArqSalvar").Value;
            _somarServico.SalvarSoma(n1, n2, caminhoCompletoArqSalvar);

            _hiLogger.ImprimirLinha("    [ Fim da Execução ]");
            _hiLogger.ImprimirLinha("    [ Hora do Fim ] " + DateTime.Now);
        }
        catch (Exception ex)
        {
            _hiLogger.ImprimirLinha("    [ Ocorreu um erro ]");
            _hiLogger.ImprimirLinha("        " + ex.Message);
            throw;
        }
    }
}