using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LFeBlog.Infrastructure.Databases
{
    public class BlogContextFactory:IDesignTimeDbContextFactory<BlogContext>
    {
        public BlogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();

            optionsBuilder.UseSqlServer("Database=BlogDb;uid=sa;password=123456; ");
            
            return new BlogContext(optionsBuilder.Options);
        }
    }
}