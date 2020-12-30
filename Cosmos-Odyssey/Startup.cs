using Cosmos_Odyssey.Helpers;
using Cosmos_Odyssey.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cosmos_Odyssey.Data;
using Microsoft.EntityFrameworkCore;

namespace Cosmos_Odyssey
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHttpClient<IApiService, ApiService>();
            services.AddTransient<IApiService, ApiService>();
            services.AddSingleton<IDatabaseUpdater, DatabaseUpdater>();
            services.AddTransient<IDataService, DataService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            InitializeDatabase(app);
            StartDatabaseUpdaterService(app);
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScope != null)
            {
                var databaseContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                databaseContext.Database.Migrate();
            }
        }

        private static void StartDatabaseUpdaterService(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScope != null)
            {
                var priceListUpdater = serviceScope.ServiceProvider.GetRequiredService<IDatabaseUpdater>();

                priceListUpdater.StartAsync();
            }
        }
    }
}
