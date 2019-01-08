using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LFeBlog.Core.Entities;

namespace LFeBlog.Core.IRepositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPostsAsync();
    }
}