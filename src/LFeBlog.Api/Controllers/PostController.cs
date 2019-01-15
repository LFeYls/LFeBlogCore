using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LFeBlog.Core.Entities;
using LFeBlog.Core.IRepositories;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;
using LFeBlog.Infrastructure.Extensions;
using LFeBlog.Infrastructure.Services;
using LFeBlog.Web.Core.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PostController(IPostRepository postRepository,
            IRepositoryService<Post> repositoryService,
            IUnitOfWork unitOfWork,
            ILogger<PostController> logger,
            IMapper mapper,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _postRepository = postRepository;
            _repositoryService = repositoryService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingContainer = propertyMappingContainer;
        }


        // GET

        [HttpGet(Name = "GetPosts")]
        [RequestHeaderMatchingMediaType("Accept", new[] {"application/vnd.cgzl.hateoas+json"})]
        public async Task<IActionResult> GetHateoas(PostParameters postParameters)
        {
            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostDto, Post>(postParameters.OrderBy))
            {
                return BadRequest("Can't finds fields for sorting");
            }

            if (!_typeHelperService.TypeHasProperties<PostDto>(postParameters.Fields))
            {
                return BadRequest("Fields not exist.");
            }

            var postList = await _postRepository.GetAllPostsAsync(postParameters);
            var postDtos = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(postList);

            var shapedPostDtos = postDtos.ToDynamicIenumerable(postParameters.Fields);
            var shapedWithLinks = shapedPostDtos.Select(x =>
            {
                var dict = x as IDictionary<string, object>;
                var postLinks = CreateLinksForPost((int) dict["id"], postParameters.Fields);
                dict.Add("links", postLinks);
                return dict;
            });

            var links = CreateLinksForPosts(postParameters, postList.HasPrevious, postList.HasNextPage);
            var result = new
            {
                values = shapedWithLinks,
                links
            };

            var meta = new
            {
                postList.PageSize,
                postList.PageIndex,
                postList.TotalItemsCount,
                postList.PageCount
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(result);

        }



        [HttpGet(Name = "GetPosts")]
        [RequestHeaderMatchingMediaType("Accept", new[] { "application/json" })]
        public async Task<IActionResult> Get(PostParameters postParameters)
        {
            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostDto, Post>(postParameters.OrderBy))
            {
                return BadRequest("Can't finds for sorting");
            }

            if (!_typeHelperService.TypeHasProperties<PostDto>(postParameters.Fields))
            {
                return BadRequest("Fields not exit");
            }

            var postList = await _postRepository.GetAllPostsAsync(postParameters);

            var postDtos = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(postList);

            var previousPageLink = postList.HasPrevious
                ? CreatePostUri(postParameters, PaginationResourceUriType.PreviousPage)
                : null;

            var nextPageLink = postList.HasNextPage
                ? CreatePostUri(postParameters, PaginationResourceUriType.NextPage)
                : null;

            var meta = new
            {
                postList.TotalItemsCount,
                postList.PageSize,
                postList.PageIndex,
                postList.PageCount,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(postDtos.ToDynamicIenumerable(postParameters.Fields));

        }


        [HttpGet("{id}", Name = "GetPost")]
        public async Task<IActionResult> Get(int id, string fields = null)
        {
            if (!_typeHelperService.TypeHasProperties<PostDto>(fields))
            {
                return BadRequest("Fields not exist.");
            }

            var post = await _postRepository.GetPostByIdAsync(id);

            if (post==null)
            {
                return NotFound();
            }

            var postDto = _mapper.Map<Post, PostDto>(post);

            var shapedPostDto = postDto.TODynamic(fields);
            var links = CreateLinksForPost(id, fields);
            var result = (IDictionary<string, object>) shapedPostDto;
            
            result.Add("links",links);

            return Ok(result);

        }


        [HttpPost(Name = "CreatePost")]
        [RequestHeaderMatchingMediaType("Content-Type",new[]{"application/vnd.cgzl.post.create+json"})]
        [RequestHeaderMatchingMediaType("Accept",new[]{"application/vnd.cgzl.hateoas+json"})]
        public async Task<IActionResult> Post([FromBody] CreateOrUpdatePostDto createOrUpdatePostDto)
        {
            if (createOrUpdatePostDto ==null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            var newPost = _mapper.Map<CreateOrUpdatePostDto, Post>(createOrUpdatePostDto);

            newPost.LastModifiedTime = DateTime.Now;
            if (createOrUpdatePostDto.Id.HasValue)
            {
                _postRepository.Update(newPost);
            }
            else
            {
                _postRepository.AddPost(newPost);
            }
            

            if (!await _unitOfWork.SaveChangeAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resultDto = _mapper.Map<Post, CreateOrUpdatePostDto>(newPost);
            var links = CreateLinksForPost(newPost.Id);
            var linkedPostDto = resultDto.TODynamic() as IDictionary<string, object>;
            linkedPostDto.Add("links",links);


            return CreatedAtRoute("GetPost", new {id = linkedPostDto["Id"]}, linkedPostDto);

            
            
        }


        [HttpPut("id",Name = "UpdatePost")]
        [RequestHeaderMatchingMediaType("Content-Type",new []{"application/vnd.cgzl.post.update+json"})]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] CreateOrUpdatePostDto createOrUpdatePostDto)
        {

            if (createOrUpdatePostDto==null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post==null)
            {
                return NotFound();
                
            }

            post.LastModifiedTime = DateTime.Now;

            _mapper.Map(createOrUpdatePostDto, post);

            if (!await _unitOfWork.SaveChangeAsync())
            {
                throw new Exception($"Updating post {id} failed when saving.");
            }

            return NoContent();

        }

        [HttpDelete("{id}",Name = "DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post==null)
            {
                return NotFound();
                
            }

            _postRepository.Delete(post);

            if (!await  _unitOfWork.SaveChangeAsync())
            {
                throw new Exception($"Deleting post {id} failed when saving.");
            }

            return NoContent();

        }


        [HttpPatch("{id}", Name = "PartiallyUpdatePost")]
        public async Task<IActionResult> PartiallyUpdateCityForCountry(int id,
            [FromBody] JsonPatchDocument<CreateOrUpdatePostDto> patchDto)
        {
            if (patchDto==null)
            {
                return BadRequest();
                
            }

            var post = await _postRepository.GetPostByIdAsync(id);

            if (post==null)
            {
                return NotFound();
            }

            var postToPatch = _mapper.Map<CreateOrUpdatePostDto>(post);

            patchDto.ApplyTo(postToPatch,ModelState);

            TryValidateModel(postToPatch);
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(postToPatch, post);
            post.LastModifiedTime=DateTime.Now;
            _postRepository.Update(post);
            if (!await  _unitOfWork.SaveChangeAsync())
            {
                throw new Exception($"Patching city {id} failed when saving;");
                
            }

            return NoContent();

        }




        #region privateMethod 

        private IEnumerable<LinkedDto> CreateLinksForPost(int id, string fields = null)
        {
            var links = new List<LinkedDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkedDto(_urlHelper.Link("GetPost", new {id}), "self", "GET"));
            }
            else
            {
                links.Add(new LinkedDto(_urlHelper.Link("GetPost", new {id, fields}), "self", "POST"));
            }

            links.Add(new LinkedDto(_urlHelper.Link("DeletePost", new {id}), "delete_post", "DELETE"));
            return links;


        }


        private IEnumerable<LinkedDto> CreateLinksForPosts(PostParameters postParameters, bool hasPrevious,
            bool hasNextPage)
        {
            var links = new List<LinkedDto>
            {
                new LinkedDto(CreatePostUri(postParameters, PaginationResourceUriType.CurrentPage), "self", "GET")
            };

            if (hasPrevious)
            {
                links.Add(new LinkedDto(CreatePostUri(postParameters, PaginationResourceUriType.PreviousPage),
                    "next_page", "GET"));
            }

            if (hasNextPage)
            {
                links.Add(new LinkedDto(CreatePostUri(postParameters, PaginationResourceUriType.NextPage), "next_page",
                    "GET"));
            }

            return links;
        }

        private string CreatePostUri(PostParameters postParameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = postParameters.PageIndex - 1,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.OrderBy,
                        fields = postParameters.Fields,
                        title = postParameters.Title
                    };
                    return _urlHelper.Link("GetPost", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = postParameters.PageIndex + 1,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.OrderBy,
                        fields = postParameters.Fields,
                        title = postParameters.Title
                    };
                    return _urlHelper.Link("GetPost", nextParameters);

                default:
                    var currentParameters = new
                    {
                        pageIndex = postParameters.PageIndex,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.OrderBy,
                        fields = postParameters.Fields,
                        title = postParameters.Title
                    };
                    return _urlHelper.Link("GetPost", currentParameters);
            }
        }
        

        #endregion 
        
    }
} 