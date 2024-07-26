using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Logging;
using Quartz.Plugin.History;
using Quartz.Plugin.Management;
using QuartzDemoWebApi.Database;

namespace QuartzDemoWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var connectionString = builder.Configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException($"ConnectionString value not found in appsettings.");
        builder.Services.AddDbContextFactory<DemoDbContext>(o => { o.UseSqlServer(connectionString); });
        builder.Services.AddQuartz(q =>
        {
            q.CheckConfiguration = true;
            q.UsePersistentStore(store =>
            {
                store.UseSqlServer(sqlServer =>
                {
                    sqlServer.ConnectionString = connectionString;
                    sqlServer.TablePrefix = "quartz.QRTZ_";
                });
            
                store.UseNewtonsoftJsonSerializer();
                store.UseClustering();
                store.PerformSchemaValidation = true;
            });

            q.SchedulerName = "scheduler1";
            q.Properties["quartz.scheduler.instanceId"] = "AUTO";

            q.SetProperty("quartz.plugin.triggerHistory.type", typeof(LoggingTriggerHistoryPlugin).AssemblyQualifiedName ?? throw new InvalidOperationException($"Unavailable assembly name for {typeof(LoggingTriggerHistoryPlugin)}"));
            q.SetProperty("quartz.plugin.jobHistory.type", typeof(LoggingJobHistoryPlugin).AssemblyQualifiedName ?? throw new InvalidOperationException($"Unavailable assembly name for {typeof(LoggingJobHistoryPlugin)}"));

            q.SetProperty("quartz.plugin.shutdownHook.type", typeof(ShutdownHookPlugin).AssemblyQualifiedName ?? throw new InvalidOperationException($"Unavailable assembly name for {typeof(ShutdownHookPlugin)}"));
            q.SetProperty("quartz.plugin.shutdownHook.cleanShutdown", "true");

            var jobKey = "demo-job-id";

            q.AddJob<DemoJob>(opts => { opts.WithIdentity(jobKey); });
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("demoJob", "demoGroup")
                .WithCronSchedule("0 * * ? * *"));
        });

        builder.Services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSimpleConsole(c =>
            {
                c.SingleLine = true;
                c.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            });
        });


        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
