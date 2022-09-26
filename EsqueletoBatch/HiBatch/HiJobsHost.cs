using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;

namespace EsqueletoBatch.HiBatch;
public class HiJobsHost : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<HiCronJob> _cronJobs;
    private readonly IHiLogger _hiLogger;
    private IScheduler? _scheduler;

    public HiJobsHost(ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        IEnumerable<HiCronJob> cronJobs,
        IHiLogger hiLogger)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _cronJobs = cronJobs;
        _hiLogger = hiLogger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        _scheduler.JobFactory = _jobFactory;

        foreach (var iCronJob in _cronJobs)
        {
            await _scheduler.ScheduleJob(iCronJob.ObterJobDetail(), iCronJob.ObterTrigger(), cancellationToken);
        }

        await _scheduler.Start(cancellationToken);

        var jobGroupNames = await _scheduler.GetJobGroupNames();
        if (!_scheduler.IsStarted)
        {
            return;
        }

        _hiLogger.ImprimirLinha("<> [ Listando Jobs agendados... ] </>");
        foreach (var iJobGroup in jobGroupNames)
        {
            var groupMatcher = GroupMatcher<JobKey>.GroupContains(iJobGroup);
            var jobKeys = await _scheduler.GetJobKeys(groupMatcher);

            foreach (var iJobKey in jobKeys)
            {
                var jobDetail = await _scheduler.GetJobDetail(iJobKey);
                var triggers = await _scheduler.GetTriggersOfJob(iJobKey);
                if (jobDetail != null)
                {
                    _hiLogger.ImprimirLinha("    [ Job " + jobDetail.JobType.Name + " ]");
                }
                else
                {
                    _hiLogger.ImprimirLinha("    [ Job não identificado ]");
                }
                foreach (var iTrigger in triggers)
                {
                    var nextFireTimeUtc = iTrigger.GetNextFireTimeUtc();
                    if (nextFireTimeUtc.HasValue)
                    {
                        _hiLogger.ImprimirLinha("        Próxima Exec = " + nextFireTimeUtc.Value.LocalDateTime.ToString());
                    }
                    var previousFireTimeUtc = iTrigger.GetPreviousFireTimeUtc();
                    if (previousFireTimeUtc.HasValue)
                    {
                        _hiLogger.ImprimirLinha("        Exec Anterior = " + previousFireTimeUtc.Value.LocalDateTime.ToString());
                    }
                }
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_scheduler != null)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}