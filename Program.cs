using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseDatabaseWebApi;
using PoseDatabaseWebApi.Service;
using PoseDatabaseWebApi.Data.Identity;
using PoseDatabaseWebApi.Data.App;
using PoseDatabaseWebApi.Models.Identity;
using Serilog;

string corsPolicyName = "AllowSeshBuilderFrontEnd";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/logs-.json", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Sesh Builder");

    var builder = WebApplication.CreateBuilder(args);

    string connectionString = ConfigurationHelper.GetConnectionString("App");
    string identityConnectionString = ConfigurationHelper.GetConnectionString("Identity");

    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

    builder.Services.AddProblemDetails(configure =>
    {
        configure.CustomizeProblemDetails = context => context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddHealthChecks();

    // Add services to the container.
    // Register NpgsqlDataSource as a singleton(DI will dispose at shutdown)
    builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));

    // Register data + service layers
    builder.Services.AddScoped<IPoseDataAccess, PoseDataAccess>();
    builder.Services.AddScoped<IPoseWebService, PoseWebService>();
    builder.Services.AddScoped<ISequenceDataAccess, SequenceDataAccess>();
    builder.Services.AddScoped<ISequenceService, SequenceService>();
    builder.Services.AddScoped<ISessionService, SessionService>();
    builder.Services.AddScoped<ISessionDataAccess, SessionDataAccess>();

    builder.Services.AddDataProtection();
    builder.Services.AddDbContext<AppUsersDbContext>(options =>
        options.UseNpgsql(identityConnectionString, options => { options.SetPostgresVersion(18, 0); }));

    builder.Services.AddAuthorization();
    builder.Services.AddIdentityApiEndpoints<AppUserModel>()
        .AddEntityFrameworkStores<AppUsersDbContext>();


    // inject automapper
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddMaps(typeof(Program).Assembly); // Scans the assembly where Program is located
                                               // Or cfg.AddMaps(typeof(SomeClassInAnotherAssembly).Assembly); to scan a different project
    });

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: corsPolicyName,
            builder =>
            {
                builder.WithOrigins("http://localhost:5174", "http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseCors(corsPolicyName);

    app.UseExceptionHandler();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecks("/health");
    app.MapControllers();
    app.MapIdentityApi<AppUserModel>();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "An error occurred while starting the application.");
}
finally
{
    Log.CloseAndFlush();
}

