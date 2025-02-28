using Expensifier.IntegrationTests;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(TestInfrastructure))]

namespace Expensifier.IntegrationTests;

public class TestInfrastructure : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    public string PostgresConnectionString => _postgres.GetConnectionString();


    public async ValueTask DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();
    }
}