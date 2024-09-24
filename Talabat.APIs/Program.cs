using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middelwares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IRepositories;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddDbContext<TalabatContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });



            builder.Services.AddSingleton<IConnectionMultiplexer>(S => {
                var connectionString = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(connectionString);
            });

            builder.Services.AddApplicationServices();

            IConfiguration configuration = builder.Configuration;

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerServices();
            builder.Services.AddIdentityService(configuration);

            builder.Services.AddCors(options => {
                options.AddPolicy("CorsPolicy", options => { 
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowAnyOrigin();
                });
            });

            var app = builder.Build();


            //Update database in program.cs file
            using (var scope = app.Services.CreateScope())
            {
                var servicesProvider = scope.ServiceProvider;
                var loggerFactory = servicesProvider.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = servicesProvider.GetRequiredService<TalabatContext>();
                    await context.Database.MigrateAsync(); //Update Database

                    var identityContext = servicesProvider.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();

                    await TalabatSeedContext.SeedAsync(context, loggerFactory);

                    var userManager = servicesProvider.GetRequiredService<UserManager<AppUser>>();
                    await AppIdentitySeedDbContext.SeedUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message);
                }
            }
            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddelware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
