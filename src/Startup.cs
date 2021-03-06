using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoseDatabaseWebApi.Models;
using Npgsql;
using AutoMapper;
using System;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PoseDatabaseWebApi
{
    public class Startup
    {
        readonly string PoseDatabaseClientOrigin = "_poseDatabaseClientOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Configuration.GetValue<string>("Value"));
            Console.ForegroundColor = ConsoleColor.White;
            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            builder.Username = Configuration["UserId"];
            builder.Password = Configuration["Password"];

            services.AddCors(options =>
            {
                options.AddPolicy(name: PoseDatabaseClientOrigin,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200");
                                  });
            });
            services.AddScoped<IPoseRepository, PoseRepository>();
            services.AddDbContext<PoseContext>(opt => opt.UseNpgsql(builder.ConnectionString));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>
            {                    
                opt.Audience = Configuration["ResourceId"];
                opt.Authority = $"{Configuration["Instance"]}{Configuration["TenantId"]}";
                opt.RequireHttpsMetadata = true;
            });
            
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PoseContext context)
        {
            //context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(PoseDatabaseClientOrigin);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
