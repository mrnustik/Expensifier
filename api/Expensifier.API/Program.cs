using Expensifier.API.Accounts;
using Expensifier.API.Common.Users;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];

if (tracingOtlpEndpoint != null)
{
    var openTelemetryBuilder = builder.Services.AddOpenTelemetry();

    openTelemetryBuilder.ConfigureResource(r => r.AddService("expensifier-api"));

    openTelemetryBuilder.WithMetrics(m =>
                                         m.AddAspNetCoreInstrumentation()
                                          .AddMeter("*")
                                          .AddPrometheusExporter());
    openTelemetryBuilder.WithTracing(t => t.AddAspNetCoreInstrumentation()
                                           .AddOtlpExporter(e =>
                                           {
                                               e.Protocol = OtlpExportProtocol.Grpc;
                                               e.Endpoint = new Uri(tracingOtlpEndpoint);
                                           }));

    builder.Logging.AddOpenTelemetry(o =>
                                         o.AddOtlpExporter(e =>
                                          {
                                              e.Protocol = OtlpExportProtocol.Grpc;
                                              e.Endpoint = new Uri(tracingOtlpEndpoint);
                                          })
                                          .IncludeScopes = true);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserProvider, FakeUserProvider>();

builder.Services.AddMediatR(options => { options.RegisterServicesFromAssemblyContaining<Program>(); });
builder.Services.AddMarten(options =>
       {
           var connectionString = builder.Configuration.GetConnectionString("Postgres");
           if (connectionString == null)
           {
               throw new InvalidOperationException("Missing connection string for Postgres");
           }

           options.Connection(connectionString);
           options.UseSystemTextJsonForSerialization(configure: configure => { configure.IncludeFields = true; });

           if (builder.Environment.IsDevelopment())
           {
               options.AutoCreateSchemaObjects = AutoCreate.All;
           }

           options.ConfigureAccounts();
       })
       .AddAsyncDaemon(DaemonMode.Solo);

builder.Services
       .AddHealthChecks()
       .AddNpgSql(builder.Configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException());


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(tracingOtlpEndpoint != null) 
{
    app.MapPrometheusScrapingEndpoint();
}
app.AddAccountEndpoints();
app.MapHealthChecks("/api/health/full");
app.MapHealthChecks("/api/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogCritical("Penis");

app.Run();