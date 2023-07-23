using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Mappings;
using PersonalFinanceManagement.Service;
using PersonalFinanceManagement.Service.Implementation;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            services.AddDbContext<PfmDbContext>(options =>
            {
                options.UseNpgsql(CreateConnectionString(Configuration));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Added TransactionService and TransactionRepository
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //Added TransactionService and TransactionRepository
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<ICsvParserService, CsvParserService>();
            services.AddScoped<ISpendingAnalyticsService, SpendingAnalyticsService>();

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonalFinanceManagement", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PfmDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalFinanceManagement v1"));
            }
            dbContext.Database.Migrate();

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
            var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "pfm";
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
