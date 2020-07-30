using System;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neutron.Application;
using Neutron.Core;
using Neutron.Infrastructure;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEventRepository, EFEventRepository>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Neutron.Application"));

            services.AddAutoMapper(it =>
            {
                it.CreateMap<CreateEventViewModel, CreateEvent>();
            },
            Assembly.GetExecutingAssembly());

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(it => it.MapControllers());
        }
    }
}
