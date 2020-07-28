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
using Neutron.Web.Database;
using Neutron.Web.Infrastructure;
using Neutron.Web.Models;
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
            services.AddDbContext<ApplicationDbContext>(it => it.UseSqlite(_configuration.GetConnectionString("ApplicationDbContext")));

            services.AddScoped<IEventRepository, EFEventRepository>();

            services.AddMediatR(typeof(CreateEventInput));

            services.AddAutoMapper(it =>
            {
                it.CreateMap<CreateEventViewModel, CreateEventInput>();
                it.CreateMap<CreateEventOutput, Event>();
            }, Assembly.GetExecutingAssembly());

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
