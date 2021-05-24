using Faucet.API.Data;
using Faucet.API.Data.Repositories;
using Faucet.API.Extensions;
using Faucet.API.Jobs;
using Faucet.API.MailClients;
using Faucet.API.RateServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using System;

namespace Faucet.API
{
    public class Startup
    {
        private const string TestEnvironmentName = "Test";
        private const string BlockchainApiBaseUrl = "https://blockchain.info/";
        private const string FaucetDbConnectionStringSectionName = "FaucetDbSqliteConnection";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient<IBlockchainRateService, BlockchainRateService>(c =>
            {
                c.BaseAddress = new Uri(BlockchainApiBaseUrl);
            });

            services.AddDbContext<FaucetDbContext>(x => 
                x.UseSqlite(Configuration.GetConnectionString(FaucetDbConnectionStringSectionName)));

            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAdminEmailRepository, AdminEmailRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            if (!CurrentEnvironment.IsEnvironment(TestEnvironmentName))
            {
                ConfigureScheduledJobs(services);
            }

            services.AddScoped<IEmailClient, EmailClient>();

            services.Configure<EmailOptions>(Configuration.GetSection(nameof(EmailOptions)));

            ConfigureSwagger(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FaucetDbContext dataContext)
        {
            dataContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Faucet API");
                    c.RoutePrefix = "swagger";
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Faucet API",
                    Description = "Faucet API - cryptocurrency reward system",
                    Contact = new OpenApiContact
                    {
                        Name = "Elvin Asadov",
                        Email = "aemloviji@gmail.com"
                    },
                });
            });
        }

        private void ConfigureScheduledJobs(IServiceCollection services)
        {
            services
                .AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionScopedJobFactory();

                    q.AddJobAndTrigger<BitcoinGrabberJob>(Configuration);
                    q.AddJobAndTrigger<SendEmailToAdminJob>(Configuration);
                })
                .AddQuartzServer(options =>
                {
                    options.WaitForJobsToComplete = true;
                });
        }
    }
}
