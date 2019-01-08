using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LFeBlog.Web.Core.Extensions
{
    public static class ExceptionHandling
    {
        public static void UseExceptionHandling(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    var ex= context.Features.Get<IExceptionHandlerFeature>();
                    var logger = loggerFactory?.CreateLogger("LFeBlog.Web.Core.Extensions.ExceptionHandling");
                    if (loggerFactory!=null && ex!=null)
                    {
                        logger.LogError(500,ex?.Error,ex?.Error?.Message);
                    }
                
                
                    await  context.Response.WriteAsync(ex?.Error?.Message ?? "内部错误");
                });
            });




        }
        
        
    }
}