using Expensifier.API.Common.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Expensifier.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly TestInfrastructure _infrastructure;

    public CustomWebApplicationFactory(TestInfrastructure infrastructure)
    {
        _infrastructure = infrastructure;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Postgres", _infrastructure.PostgresConnectionString);
        builder.ConfigureServices(services => services.AddSingleton<IUserProvider>(new TestUserProvider()));
    }
}