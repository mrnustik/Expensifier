using Expensifier.API.Accounts;
using Marten;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddMarten(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    if (connectionString == null)
    {
        throw new InvalidOperationException("Missing connection string for Postgres");
    }
    options.Connection(connectionString);
    
    options.UseSystemTextJsonForSerialization();
    
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddAccountEndpoints();

app.Run();