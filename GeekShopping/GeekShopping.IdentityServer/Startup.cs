using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Configuration.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["MySQLConnection:MySQLConnectionString"];

            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
            
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MySQLContext>().AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            { options.Events.RaiseErrorEvents = true;
              options.Events.RaiseInformationEvents = true;
              options.Events.RaiseFailureEvents = true;
              options.Events.RaiseSuccessEvents = true;
              options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                .AddInMemoryClients(IdentityConfiguration.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            builder.AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env, IDbInitializer initializer)
        {
            // Configure the HTTP request pipeline. //Configure()
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            initializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
