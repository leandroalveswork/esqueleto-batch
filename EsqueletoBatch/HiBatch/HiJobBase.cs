using Microsoft.Extensions.Configuration;

namespace EsqueletoBatch.HiBatch;
public class HiJobBase
{
    protected readonly Guid _guid;
    protected readonly IHiLogger _hiLogger;
    protected readonly IConfigurationRoot _config;
    public HiJobBase(IHiLogger hiLogger, IConfigurationRoot config)
    {
        _guid = Guid.NewGuid();
        _hiLogger = hiLogger;
        _config = config;
    }
    protected void ImprimirInicioExec(string tituloExec)
    {
        _hiLogger.ImprimirLinha("<> [ EXECUÇÃO: " + tituloExec + " ] </>");
        _hiLogger.ImprimirLinha("    [ Guid ] " + _guid);
        _hiLogger.ImprimirLinha("    [ Hora do Início ] " + DateTime.Now);
        _hiLogger.ImprimirLinha("    [ Início da Execução... ]");
    }
    protected void ImprimirFimExec()
    {
        _hiLogger.ImprimirLinha("    [ Fim da Execução ]");
        _hiLogger.ImprimirLinha("    [ Hora do Fim ] " + DateTime.Now);
    }
    protected void ImprimirErro(Exception ex)
    {
        _hiLogger.ImprimirLinha("    [ Ocorreu um erro ]");
        _hiLogger.ImprimirLinha("        " + ex.Message);
    }
}