using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LFeBlog.Infrastructure.Databases;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace LFeBlog.Web.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("Logs",@"log.txt"),rollingInterval:RollingInterval.Day)
                .CreateLogger();
            
            
          var host=  CreateWebHostBuilder(args).Build();


          using (var scope= host.Services.CreateScope())
          {
              var services = scope.ServiceProvider;
              var loggerFactory = services.GetRequiredService<ILoggerFactory>();

              try
              {
                  BlogContextSeed.SeedAsync(services.GetRequiredService<BlogContext>(), loggerFactory).Wait();
              }
              catch (Exception e)
              {
                  var logger = loggerFactory.CreateLogger<Program>();
                  
                  logger.LogError(e,"初始化数据失败");
              }
                  
          }
          
          
          host.Run();
          
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}