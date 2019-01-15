using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.Databases;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;
using LFeBlog.Infrastructure.Repositories;
using LFeBlog.Infrastructure.Services;
using LFeBlog.Infrastructure.Validations.PostValidation;
using LFeBlog.Web.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

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
//                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                var inputFormatter = options.InputFormatters.OfType<JsonInputFormatter>().FirstOrDefault();
                if (inputFormatter != null)
                {
                    inputFormatter.SupportedMediaTypes.Add("application/vnd.cgzl.post.create+json");
                    inputFormatter.SupportedMediaTypes.Add("applicationvnd.cgzl.post.update+json");
                    
                }

                var outputFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
                if (outputFormatter!=null)
                {
                    outputFormatter.SupportedMediaTypes.Add("application/vnd.cgzl.hateoas+json");
                }
            })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =new CamelCasePropertyNamesContractResolver();
                })
                .AddFluentValidation();
            services.AddDbContext<BlogContext>(options =>
            {
                
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });

            services
                .AddTransient<IValidator<CreateOrUpdatePostDto>, CreateOrUpdatePostDtoValidator<CreateOrUpdatePostDto>
                >();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            
            services.AddAutoMapper();
            
            
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped(typeof(IRepositoryService<>), typeof(RepositoryBaseService<>));

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<PostPropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            services.AddTransient<ITypeHelperService, TypeHelperService>();

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