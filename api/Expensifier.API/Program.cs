using Expensifier.API.Accounts;
using Expensifier.API.Categories;
using Expensifier.API.Common.Users;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

var oltpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];

var openTelemetryBuilder = builder.Services.AddOpenTelemetry();

openTelemetryBuilder.ConfigureResource(r => r.AddService("expensifier-api"));

openTelemetryBuilder.WithMetrics(m =>
                                     m.AddAspNetCoreInstrumentation()
                                      .AddRuntimeInstrumentation()
                                      .AddProcessInstrumentation()
                                      .AddNpgsqlInstrumentation()
                                      .AddMeter("Marten")
                                      .AddMeter("*")
                                      .AddPrometheusExporter());

openTelemetryBuilder.WithTracing(t =>
{
    t.AddAspNetCoreInstrumentation()
     .AddNpgsql()
     .AddSource("Marten");
    if (!string.IsNullOrEmpty(oltpEndpoint))
    {
        t.AddOtlpExporter(e =>
        {
            e.Protocol = OtlpExportProtocol.Grpc;
            e.Endpoint = new Uri(oltpEndpoint);
        });
    }
});

builder.Logging.AddOpenTelemetry(o =>
{
    o.IncludeScopes = true;

    if (!string.IsNullOrEmpty(oltpEndpoint))
    {
        o.AddOtlpExporter(e =>
        {
            e.Protocol = OtlpExportProtocol.Grpc;
            e.Endpoint = new Uri(oltpEndpoint);
        });
    }
});

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
           options.OpenTelemetry.TrackConnections = TrackLevel.Verbose;
           options.OpenTelemetry.TrackEventCounters();
           options.ConfigureAccounts();
           options.RegisterValueType(typeof(UserId));
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

app.MapPrometheusScrapingEndpoint();
app.MapCategoryEndpoints();
app.AddAccountEndpoints();
app.MapHealthChecks("/api/health/full");
app.MapHealthChecks("/api/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();

public partial class Program { }