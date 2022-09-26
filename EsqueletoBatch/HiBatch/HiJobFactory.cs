using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace EsqueletoBatch.HiBatch;
public class HiJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;
    public HiJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService<HiJobRunner>();
    }

    public void ReturnJob(IJob job)
    {
        
    }
}