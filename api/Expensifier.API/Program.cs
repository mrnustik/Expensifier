using Expensifier.API.Accounts;
using Expensifier.API.Common.Users;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddAccountEndpoints();

app.Run();