using Azure.Monitor.OpenTelemetry.AspNetCore;

using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTestLog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry().UseAzureMonitor(o => {
    o.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<IServiceBus, ServiceBus>();

builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlServerOptionsAction: sqlOptions => {
                sqlOptions.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
            });
    options.ConfigureLoggingCacheTime(TimeSpan.FromMinutes(15));
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSerilogRequestLogging();

Log.Information("Application Starting up");
app.Run();
