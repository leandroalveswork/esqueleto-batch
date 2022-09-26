using Quartz;

namespace EsqueletoBatch.HiBatch;
public class HiCronJob
{
    public Type TypeofJob { get; private set; }
    public string Cron { get; private set; }
    public HiCronJob(Type typeofJob, string cron)
    {
        TypeofJob = typeofJob;
        Cron = cron;
    }
    
    public IJobDetail ObterJobDetail()
    {
        return JobBuilder.Create(TypeofJob)
            .WithIdentity(TypeofJob.FullName ?? "")
            .WithDescription(TypeofJob.Name)
            .Build();
    }
    
    public ITrigger ObterTrigger()
    {
        return TriggerBuilder.Create()
            .WithIdentity(TypeofJob.FullName + ".trigger")
            .WithCronSchedule(Cron)
            .WithDescription(Cron)
            .Build();
    }
}