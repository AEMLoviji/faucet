using Faucet.API.Data;
using Faucet.API.Db;
using Faucet.API.RateServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Faucet.API
{
    public class Startup
    {
        private const string BlockchainApiBaseUrl = "https://blockchain.info/";
        private const string FaucetDbConnectionStringName = "FaucetDbSqliteConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient<IBlockchainRateService, BlockchainRateService>(c =>
            {
                c.BaseAddress = new Uri(BlockchainApiBaseUrl);
            });

            services.AddDbContext<FaucetDbContext>(x => x.UseSqlite(Configuration.GetConnectionString(FaucetDbConnectionStringName)));

            services.AddScoped<IBalanceRepository, BalanceRepository>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
