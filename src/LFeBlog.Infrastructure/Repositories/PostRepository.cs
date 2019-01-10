using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFeBlog.Core.Entities;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.Databases;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;
using LFeBlog.Infrastructure.Extensions;
using LFeBlog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace LFeBlog.Infrastructure.Repositories
{
    public class PostRepository:IPostRepository
    {
        private readonly BlogContext _blogDbContext;

        private readonly IPropertyMappingContainer _propertyMappingContainer; 

        public PostRepository(BlogContext blogDbContext, IPropertyMappingContainer propertyMappingContainer)
        {
            _blogDbContext = blogDbContext;
            _propertyMappingContainer = propertyMappingContainer;
        }


        public async Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters)
        {
            var query = _blogDbContext.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(postParameters.Title))
            {
                var title = postParameters.Title.ToLowerInvariant();

                query = query.Where(c => c.Title.ToLowerInvariant() == title);
            }

            query = query.ApplySort(postParameters.OrderBy, _propertyMappingContainer.Resolve<PostDto, Post>());

            var count = await query.CountAsync();
            var data = await query.Skip(postParameters.PageIndex * postParameters.PageSize)
                .Take(postParameters.PageSize)
                .ToListAsync();
            
            return new PaginatedList<Post>(data,count,postParameters.PageSize,postParameters.PageIndex);

        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _blogDbContext.Posts.FindAsync(id);
        }


        public void AddPost(Post post)
        {
            _blogDbContext.Posts.Add(post);
        }

        public void Delete(Post post)
        {
            _blogDbContext.Posts.Remove(post);
        }

        public void Update(Post post)
        {
            _blogDbContext.Entry(post).State = EntityState.Modified;
        }
        
    }
}