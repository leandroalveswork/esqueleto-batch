using Microsoft.Extensions.Configuration;

namespace EsqueletoBatch.Servico;
public class SomarServico : ISomarServico
{
    private readonly IConfigurationRoot _config;
    public SomarServico(IConfigurationRoot config)
    {
        _config = config;
    }
    
    public void SalvarSoma(int n1, int n2, string caminhoCompletoArqSalvar)
    {
        var total = n1 + n2;
        File.AppendAllLines(caminhoCompletoArqSalvar, new [] { total.ToString() });
    }
}