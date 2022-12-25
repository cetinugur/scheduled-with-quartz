using Microsoft.Extensions.DependencyInjection;
using ScheduledJobs.App.Core.Engines;
using ScheduledJobs.App.Core.Extensions;

try
{
    var services = new ServiceCollection();
    services.AddScheduledJobEngines();

    await using var scope = services.BuildServiceProvider().CreateAsyncScope();
    scope.ServiceProvider.GetRequiredService<JobEngineAppCore>().Run();
}
catch (Exception exp)
{
    Console.WriteLine($"Uygulama başlatılamadı : {exp.Message}");
}

Console.ReadLine();