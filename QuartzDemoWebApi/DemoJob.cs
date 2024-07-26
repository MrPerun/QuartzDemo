using Quartz;

namespace QuartzDemoWebApi;

public class DemoJob : IJob
{
    private readonly ILogger<DemoJob> logger;

    public DemoJob(ILogger<DemoJob> logger)
    {
        this.logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Running the job");
        return Task.CompletedTask;
    }
}
