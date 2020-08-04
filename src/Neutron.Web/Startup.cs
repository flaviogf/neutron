using System;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neutron.Application;
using Neutron.Core;
using Neutron.Infrastructure;
using Neutron.Web.Hubs;
using Neutron.Web.ViewModels;

namespace Neutron.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NeutronDbContext>(it => it.UseSqlite(_configuration.GetConnectionString("NeutronDbContext"), b => b.MigrationsAssembly("Neutron.Web")));

            services.AddDbContext<IdentityDbContext>(it => it.UseSqlite(_configuration.GetConnectionString("IdentityDbContext"), b => b.MigrationsAssembly("Neutron.Web")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(it =>
            {
                it.Password.RequireDigit = false;
                it.Password.RequiredLength = 6;
                it.Password.RequiredUniqueChars = 1;
                it.Password.RequireLowercase = false;
                it.Password.RequireNonAlphanumeric = false;
                it.Password.RequireUppercase = false;
            });

            services.ConfigureApplicationCookie(it =>
            {
                it.Cookie.HttpOnly = true;
                it.LoginPath = "/SignIn";
                it.AccessDeniedPath = "/SignIn";
                it.SlidingExpiration = true;
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEventRepository, EFEventRepository>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Neutron.Application"));

            services.AddAutoMapper(it =>
            {
                it.CreateMap<CreateEventViewModel, CreateEvent>();
            },
            Assembly.GetExecutingAssembly());

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(it =>
            {
                it.MapControllers();

                it.MapHub<EventHub>("/eventHub");
            });
        }
    }
}
