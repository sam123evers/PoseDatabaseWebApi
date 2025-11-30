using Npgsql;
using Microsoft.AspNetCore.Identity;
using PoseDatabaseWebApi;
using PoseDatabaseWebApi.Data;
using PoseDatabaseWebApi.Service;
using PoseDatabaseWebApi.Data.Identity;

string connectionString = ConfigurationHelper.GetConnectionString("PoseDatabase");

await using var conn = new NpgsqlConnection(connectionString);

await using var dataSource = NpgsqlDataSource.Create(connectionString);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register NpgsqlDataSource as a singleton(DI will dispose at shutdown)
builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));

// Register data + service layers
builder.Services.AddScoped<IPoseDataAccess, PoseDataAccess>();
builder.Services.AddScoped<IPoseWebService, PoseWebService>();
builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { 

//})
//.AddDefaultTokenProviders();

builder.Services.AddDataProtection();

builder.Services.AddIdentityCore<IdentityUser>(options => { })
    .AddUserStore<UserDataAccess>()
    .AddDefaultTokenProviders();


// register password hasher
builder.Services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();

// register your custom Postgres stores
builder.Services.AddScoped<IUserStore<IdentityUser>, UserDataAccess>();
//builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleDataAccess>();

// inject automapper
builder.Services.AddAutoMapper(typeof(Program));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
