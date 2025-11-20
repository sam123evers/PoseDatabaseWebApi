using Microsoft.Win32;
using Npgsql;
using PoseDatabaseWebApi;
using PoseDatabaseWebApi.Data;
using PoseDatabaseWebApi.Service;

string connectionString = ConfigurationHelper.GetConnectionString("PoseDatabase");

await using var conn = new NpgsqlConnection(connectionString);
//await conn.OpenAsync();
//Console.WriteLine($"The PostgreSQL version: {conn.PostgreSqlVersion}");

await using var dataSource = NpgsqlDataSource.Create(connectionString);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register NpgsqlDataSource as a singleton(DI will dispose at shutdown)
builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));

// Register data + service layers
builder.Services.AddScoped<IPoseWebData, PoseWebData>();
builder.Services.AddScoped<IPoseWebService, PoseWebService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
