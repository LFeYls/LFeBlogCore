using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.Databases;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;
using LFeBlog.Infrastructure.Repositories;
using LFeBlog.Infrastructure.Validations.PostValidation;
using LFeBlog.Web.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LFeBlog.Web.Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<Startup> _logger;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration,
            ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable=true;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });
            services.AddDbContext<BlogContext>(options =>
            {
                
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped(typeof(IRepositoryService<>), typeof(RepositoryBaseService<>));
//            services.AddSingleton<BlogContext>();
            services.AddAutoMapper();
//            services.AddTransient(typeof(IValidator<>),typeof(AbstractValidator<>));
            services.AddTransient<IValidator<PostDto>, PostValidation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

//            app.UseExceptionHandling(loggerFactory);
                
          
            app.UseMvc();
//            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}