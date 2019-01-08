using LFeBlog.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LFeBlog.Infrastructure.Databases
{
    public class BlogContext:DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }
        
        
       public DbSet<Post> Posts { get; set; }
    }
}