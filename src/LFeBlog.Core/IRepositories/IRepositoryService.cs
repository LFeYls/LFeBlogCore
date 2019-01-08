using System.Collections.Generic;
using System.Threading.Tasks;
using LFeBlog.Core.Interfaces;

namespace LFeBlog.Core.IRepositories
{
    public interface IRepositoryService<TEntity> where TEntity:class, IEntity
    {
        Task<IEnumerable<TEntity>> GetAll();


        Task<TEntity> InsertAndGetIdAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(int id);
    }
}