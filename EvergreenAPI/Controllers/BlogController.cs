using System;
using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public BlogController(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            var blogs = _mapper.Map<List<Blog>>(_blogRepository.GetBlogs());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(blogs);
        }


        [HttpGet("{blogId}")]
        [AllowAnonymous]
        public IActionResult GetBlog(int blogId)
        {
            if (!_blogRepository.BlogExist(blogId))
                return NotFound($"Blog Category '{blogId}' is not exists!!");

            var blogs = _mapper.Map<Blog>(_blogRepository.GetBlog(blogId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(blogs);
        }


        [HttpPost]
        public IActionResult CreateBlog([FromBody] BlogDto blogCreate)
        {
            if (blogCreate == null)
                return BadRequest(ModelState);

            var blog = _blogRepository
                .GetBlogs()
                .FirstOrDefault(c => string.Equals(c.Title.Trim(), blogCreate.Title.TrimEnd(),
                    StringComparison.CurrentCultureIgnoreCase));

            if (blog != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blogMap = _mapper.Map<Blog>(blogCreate);

            if (!_blogRepository.CreateBlog(blogMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }


        [HttpPut("{blogId}")]
        public IActionResult UpdateBlog(int blogId, [FromBody] BlogDto updatedBlog)
        {
            if (updatedBlog == null)
                return BadRequest(ModelState);

            if (blogId != updatedBlog.BlogId)
                return BadRequest(ModelState);

            if (!_blogRepository.BlogExist(blogId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blogMap = _mapper.Map<Blog>(updatedBlog);

            if (!_blogRepository.UpdateBlog(blogMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }



        [HttpPut("UpdateViewBlog/{blogId}")]
        public IActionResult UpdateViewBlog(int blogId)
        {
            

            if (!_blogRepository.UpdateViewBlog(blogId))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }



        [HttpGet("GetViewBlog/{id}")]
        public IActionResult GetViewBlog(int id)
        {
            if (_blogRepository.GetViewBlog(id) == 0)
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok(_blogRepository.GetViewBlog(id));
        }


        [HttpDelete("{blogId}")]
        public IActionResult DeleteBlog(int blogId)
        {
            if (!_blogRepository.BlogExist(blogId))
                return NotFound();

            var blogToDelete = _blogRepository.GetBlog(blogId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_blogRepository.DeleteBlog(blogToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }

            return Ok("Delete Success");
        }


        [HttpGet("Search")]
        public ActionResult<List<Blog>> Search(string search)
        {
            var list = _blogRepository.Search(search);

            return Ok(list);
        }
    }
}