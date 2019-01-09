using System.Collections.Generic;
using System.Threading.Tasks;
using LFeBlog.Core.Entities;

namespace LFeBlog.Core.IRepositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync(PostParameters postParameters);

        Task<Post> GetPostByIdAsync(int id);

        void AddPost(Post post);

        void Delete(Post post);

        void Update(Post post);



    }
}