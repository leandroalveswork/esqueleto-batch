using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace EsqueletoBatch.HiBatch;
public class HiJobRunner : IJob
{
    private readonly IServiceProvider _serviceProvider;
    public HiJobRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext jobContext)
    {
        var scope = _serviceProvider.CreateScope();
        var job = scope.ServiceProvider.GetRequiredService(jobContext.JobDetail.JobType) as IJob;
        if (job is null)
        {
            throw new NullReferenceException("Não foi possível recuperar o job " + jobContext.JobDetail.JobType.Name);
        }
        await job.Execute(jobContext);
    }
}