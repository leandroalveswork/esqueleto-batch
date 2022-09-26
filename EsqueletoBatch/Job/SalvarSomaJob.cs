using EsqueletoBatch.HiBatch;
using EsqueletoBatch.Servico;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace EsqueletoBatch.Job;
public class SalvarSomaJob : HiJobBase, IJob
{
    private readonly ISomarServico _somarServico;
    public SalvarSomaJob(ISomarServico somarServico, IHiLogger hiLogger, IConfigurationRoot config) : base(hiLogger, config)
    {
        _somarServico = somarServico;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            ImprimirInicioExec("Exportação da Soma");
            var n1 = int.Parse(_config.GetSection("Jobs:SalvarSomaJob:Params:N1").Value);
            var n2 = int.Parse(_config.GetSection("Jobs:SalvarSomaJob:Params:N2").Value);
            var caminhoCompletoArqSalvar = _config.GetSection("Jobs:SalvarSomaJob:Params:CaminhoCompletoArqSalvar").Value;
            _somarServico.SalvarSoma(n1, n2, caminhoCompletoArqSalvar);
        }
        catch (Exception ex)
        {
            _hiLogger.ImprimirLinha("    [ Ocorreu um erro ]");
            _hiLogger.ImprimirLinha("        " + ex.Message);
            throw;
        }
    }
}