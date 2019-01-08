using System.Collections.Generic;
using System.Threading.Tasks;
using LFeBlog.Core.Interfaces;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace LFeBlog.Infrastructure.Repositories
{
    public class RepositoryBaseService<TEntity>:IRepositoryService<TEntity> 
    where TEntity:class ,IEntity
    {
        private readonly BlogContext _blogContext;
        private readonly DbSet<TEntity> _table;

        public RepositoryBaseService(BlogContext blogContext)
        {
            _blogContext = blogContext;

             _table = blogContext.Set<TEntity>();
        }


        public async Task<IEnumerable<TEntity>> GetAll()
        {
          return  await _table.ToListAsync();
        }


        public async Task<TEntity> InsertAndGetIdAsync(TEntity entity)
        {
          var modal=  await _table.AddAsync(entity);
          return modal.Entity;

        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);

        }

    }
}