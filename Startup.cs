using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestAuth2Mvc.Data;
using TestAuth2Mvc.Identity;
using TestAuth2Mvc.Services;
using TestAuth2Mvc.Identity.Models;
using TestAuth2Mvc.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace TestAuth2Mvc
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
            services.Configure<LdapSettings>(Configuration.GetSection("LdapSettings"));

            services.AddDbContext<LdapDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<LdapUser, IdentityRole>()
                .AddEntityFrameworkStores<LdapDbContext>()
                .AddUserManager<LdapUserManager>()
                .AddSignInManager<LdapSignInManager>()  
                .AddDefaultTokenProviders();

services
                .ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = "TestAuth2Mvc";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = "/Account/Signin"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                    options.LogoutPath = "/Account/Signout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                    options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                    options.SlidingExpiration = true;
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                });

            // Add application services.
            services.AddTransient<ILdapService, LdapService>();

            services.AddAuthorization(options => {
                    options.AddPolicy("AdGroup-DomainAdmins", policy =>
                                    policy.RequireClaim("AdGroup", "Domain Admins"));
                });

            services.AddControllersWithViews(config => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }

            );

            services.AddDbContext<TestMvcContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("TestMvcContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
