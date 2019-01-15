using AutoMapper;
using LFeBlog.Core.Entities;

namespace LFeBlog.Infrastructure.EntityDtos.PostDtos
{
    public class PostMapping:Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostDto>().ReverseMap();

            CreateMap<CreateOrUpdatePostDto, Post>().ReverseMap();

        }
    }
}