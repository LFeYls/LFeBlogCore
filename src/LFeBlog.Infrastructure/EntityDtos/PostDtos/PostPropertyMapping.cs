using System;
using System.Collections.Generic;
using LFeBlog.Core.Entities;
using LFeBlog.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace LFeBlog.Infrastructure.EntityDtos.PostDtos
{
    public class PostPropertyMapping:PropertyMapping<PostDto,Post>
    {
        public PostPropertyMapping() : base(new Dictionary<string, List<MappedProperty>>(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(Post.Title)] =new List<MappedProperty>
            {
                new MappedProperty{Name=nameof(Post.Title),Revert = false}
            },
            [nameof(Post.Body)] =new List<MappedProperty>
            {
                new MappedProperty{Name=nameof(Post.Body),Revert = false}
            },
            [nameof(Post.Author)] =new List<MappedProperty>
            {
                new MappedProperty{Name=nameof(Post.Author),Revert = false}
            }
        })
        {
         
        }
    }
}