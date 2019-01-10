using System.Threading.Tasks;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace LFeBlog.Infrastructure.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly BlogContext _blogDbContext;

        public UnitOfWork(BlogContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<bool> SaveChangeAsync()
        {

         return  await _blogDbContext.SaveChangesAsync()>0;
        }
    }
}