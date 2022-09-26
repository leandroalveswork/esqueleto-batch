using System.ServiceProcess;
using Microsoft.Extensions.Hosting;

namespace EsqueletoBatch.HiBatch;
public class HiTempoDeVidaServico : ServiceBase, IHostLifetime
{
    public HiTempoDeVidaServico(IHostApplicationLifetime applicationLifetime)
    {
        ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        DelayStart = new TaskCompletionSource<object>();
    }

    public IHostApplicationLifetime ApplicationLifetime { get; }
    public TaskCompletionSource<object> DelayStart { get; set; }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Stop();
        return Task.CompletedTask;
    }

    protected override void OnStart(string[] args)
    {
        DelayStart.TrySetResult(null);
        base.OnStart(args);
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() => DelayStart.TrySetCanceled());
        ApplicationLifetime.ApplicationStopping.Register(Stop);
        new Thread(Run).Start();
        return DelayStart.Task;
    }

    private void Run()
    {
        try
        {
            Run(this);
            DelayStart.TrySetException(new InvalidOperationException("Parado, sem iniciar o serviço."));
        }
        catch (Exception ex)
        {
            DelayStart.TrySetException(ex);
            throw;
        }
    }
}