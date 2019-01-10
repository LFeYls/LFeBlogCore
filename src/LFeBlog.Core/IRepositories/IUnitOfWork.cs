using System.Threading.Tasks;

namespace LFeBlog.Core.IRepositories
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangeAsync();
    }
}