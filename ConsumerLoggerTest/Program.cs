using Azure.Monitor.OpenTelemetry.AspNetCore;

using ConsumerLoggerTest;
using ConsumerLoggerTest.Data;

using Microsoft.EntityFrameworkCore;

using Serilog;

using System.Globalization;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

builder.Services.AddHostedService<Worker>();
builder.Services.AddApplicationInsightsTelemetryWorkerService();
builder.Services.AddOpenTelemetry().UseAzureMonitor(o => {
    o.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});
builder.Services.AddLogging(config => {

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    config.AddSerilog(Log.Logger);

});

builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlServerOptionsAction: sqlOptions => {
                sqlOptions.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
            });
    options.ConfigureLoggingCacheTime(TimeSpan.FromMinutes(15));
});

builder.Services.AddSingleton<ISubscriptionReceiver>(service => {
    ServiceProvider provider = builder.Services.BuildServiceProvider();
    ILoggerFactory? logginFactory = provider.GetService<ILoggerFactory>();
    ApplicationContext? applicationCOntext = provider.GetService<ApplicationContext>();
    return new SubscriptionReceiver(builder.Configuration, logginFactory, applicationCOntext);
});

//builder.Services.AddOpenTelemetry().UseAzureMonitor();

IHost host = builder.Build();
host.Run();
