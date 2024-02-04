using Azure.Monitor.OpenTelemetry.AspNetCore;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry().UseAzureMonitor();
builder.Services.AddApplicationInsightsTelemetry();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSerilogRequestLogging();

try {
    Log.Information("Application Starting up");
    app.Run();
}
catch (Exception ex) {

    Log.Fatal(ex, "Occurred at error when stating the application");
}
finally {
    Log.CloseAndFlush();
}

