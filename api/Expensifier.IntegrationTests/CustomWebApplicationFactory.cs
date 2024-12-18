using JasperFx.Core.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Expensifier.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
                                                     .WithImage("postgres:latest")
                                                     .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Postgres", _postgres.GetConnectionString());
    }

    public Task StartAsync()
    {
        return _postgres.StartAsync();
    }

    public override ValueTask DisposeAsync()
    {
        base.DisposeAsync();
        return _postgres.DisposeAsync();
    }
}