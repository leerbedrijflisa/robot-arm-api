﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Lisa.RobotArm.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TableStorageSettings>(Configuration.GetSection("TableStorage"));

            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddCors();
            services.AddScoped<TableStorage>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(cors =>
            {
                cors.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            app.UseMvc();
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseIISIntegration()
              .UseStartup<Startup>()
              .Build();

            host.Run();
        }
    }
}