using System.Collections.Generic;
using System.Threading.Tasks;
using LFeBlog.Core.Entities;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace LFeBlog.Infrastructure.Repositories
{
    public class PostRepository:IPostRepository
    {
        private readonly DbContext _blogDbContext;

        public PostRepository(BlogContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }


        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
          return await _blogDbContext.Set<Post>().ToListAsync();
        }
    }
}