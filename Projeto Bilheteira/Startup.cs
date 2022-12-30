namespace Utad_Proj_
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Utad_Proj_.Data;
    using Utad_Proj_.Models;
    using Utad_Proj_.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            SeedRoles.Seed(roleManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Movies}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            // Register singleton instances for options.
            services.AddSingleton<AuthEmailSenderOptions>(sf => this.Configuration.GetSection("AuthEmailSender").Get<AuthEmailSenderOptions>());
            services.AddSingleton<PurchaseEmailSenderOptions>(sf => this.Configuration.GetSection("PurchaseEmailSender").Get<PurchaseEmailSenderOptions>());
            services.AddSingleton<NewMovieEmailSenderOptions>(sf => this.Configuration.GetSection("NewMovieEmailSender").Get<NewMovieEmailSenderOptions>());

            services.AddSingleton<IEmailSenderService, EmailSenderService>(sf => new EmailSenderService(this.Configuration.GetSection("SendGrid").Get<SendGridEmailOptions>()));
            services.AddTransient<IEmailSender, AuthEmailSender>();

            services.AddTransient<IPurchaseService, PurchaseService>();

            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(25); //session can only be idle for 25minutes
                option.Cookie.Name = ".App.Session"; //this is the default name but it can be changed
            });
        }
    }
}