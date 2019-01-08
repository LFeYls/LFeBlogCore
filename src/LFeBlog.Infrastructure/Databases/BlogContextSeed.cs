using System;
using System.Threading.Tasks;
using LFeBlog.Core.Entities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace LFeBlog.Infrastructure.Databases
{
    public class BlogContextSeed
    {

        public static async Task SeedAsync(BlogContext dbContext, ILoggerFactory loggerFactory)
        {

            if (!dbContext.Posts.Any())
            {
                var posts = new Post[]
                {
                    new Post
                    {
                        Title = "爵士舞功",
                        Author = "张三丰",
                        Body = "最近有研究了一门绝学",
                        LastModifiedTime = DateTime.Now
                    },

                    new Post
                    {
                        Title = "久仰神功",
                        Body = "关于九阳神功那是不传之绝学，不过久仰神功还是可以公开的",
                        Author = "张无忌",
                        LastModifiedTime = DateTime.Now
                    },
                    new Post
                    {
                        Title = "圆周率",
                        Body = "关于圆周率呢，我还是有点见解的",
                        Author = "祖冲之",
                        LastModifiedTime = DateTime.Now
                    }, 
                };

                try
                {
                    await dbContext.Posts.AddRangeAsync(posts);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    var logger= loggerFactory.CreateLogger<BlogContextSeed>();
                    logger.Log(LogLevel.Error,"生成种子数据失败");

                }
            }
            
            
            
            
            
        }
        
        
    }
}