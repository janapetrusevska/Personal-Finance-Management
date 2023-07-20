using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Mappings;
using PersonalFinanceManagement.Service;
using PersonalFinanceManagement.Service.Implementation;
using System;
using System.Reflection;

namespace PersonalFinanceManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Added TransactionDbContext
            services.AddDbContext<TransactionDbContext>(options =>
            {
                options.UseNpgsql(CreateConnectionString(Configuration));
                //options.UseSqlServer(CreateConnectionString(Configuration));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Added TransactionService and TransactionRepository
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonalFinanceManagement", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalFinanceManagement v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public string CreateConnectionString(IConfiguration configuration)
        {
            var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "JanaPetrovec54";
            var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "PFM";
            var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";

            var connectionBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = int.Parse(port),
                Username = username,
                Database = databaseName,
                Password = password,
                Pooling = true
            };

            string connectionString = connectionBuilder.ConnectionString;

            return connectionString;
        }

    }
}
