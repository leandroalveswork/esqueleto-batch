namespace EsqueletoBatch.HiBatch;
public class HiConsoleLogger : IHiLogger
{
    public void ImprimirLinha(string linha)
    {
        Console.WriteLine(linha);
    }
}