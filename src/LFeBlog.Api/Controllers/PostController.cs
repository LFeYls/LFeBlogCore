using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LFeBlog.Core.Entities;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LFeBlog.Web.Core.Controllers
{
    
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IRepositoryService<Post> _repositoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository,
            IRepositoryService<Post> repositoryService,
            IUnitOfWork unitOfWork,
            ILogger<PostController> logger,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _repositoryService = repositoryService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add()
        {
            var post=new Post
            {
                Title = "测试标题",
                Body = "测试Body",
                Author = "本人",
                LastModifiedTime = DateTime.Now
            };
            var modal= await _repositoryService.InsertAndGetIdAsync(post);
            await _unitOfWork.SaveChangeAsync();

            return Ok(modal);

        }
        // GET
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            var post = await _repositoryService.GetAll();
            _logger.LogInformation("get all Posts");

           var dtoList= _mapper.Map<IEnumerable<Post>,IEnumerable<PostDto>>(post);
            return new JsonResult(dtoList);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _repositoryService.GetByIdAsync(id);

            if (post==null)
            {
                return NotFound();
            }
            var dto=_mapper.Map<Post, PostDto>(post);

            return Ok(dto);
        }


    

    }
}