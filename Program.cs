using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseDatabaseWebApi;
using PoseDatabaseWebApi.Service;
using PoseDatabaseWebApi.Data.Identity;
using PoseDatabaseWebApi.Data.App;
using PoseDatabaseWebApi.Models.Identity;

string connectionString = ConfigurationHelper.GetConnectionString("App");
string identityConnectionString = ConfigurationHelper.GetConnectionString("Identity");

await using var conn = new NpgsqlConnection(connectionString);

await using var dataSource = NpgsqlDataSource.Create(connectionString);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register NpgsqlDataSource as a singleton(DI will dispose at shutdown)
builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));

// Register data + service layers
builder.Services.AddScoped<IPoseDataAccess, PoseDataAccess>();
builder.Services.AddScoped<IPoseWebService, PoseWebService>();
builder.Services.AddScoped<ISequenceDataAccess, SequenceDataAccess>();
builder.Services.AddScoped<ISequenceService, SequenceService>();
//builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppUsersDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { 

//})
//.AddDefaultTokenProviders();

builder.Services.AddDataProtection();
builder.Services.AddDbContext<AppUsersDbContext>(options => 
    options.UseNpgsql(identityConnectionString, options => { options.SetPostgresVersion(18,0); }));

//builder.Services.AddIdentityCore<IdentityUser>(options => { })
//    .AddUserStore<UserDataAccess>()
//    .AddDefaultTokenProviders();


// register password hasher
//builder.Services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();

// register your custom Postgres stores
//builder.Services.AddScoped<IUserStore<IdentityUser>, UserDataAccess>();
//builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleDataAccess>();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUserModel>()
    .AddEntityFrameworkStores<AppUsersDbContext>();

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
app.MapIdentityApi<AppUserModel>();

app.Run();
