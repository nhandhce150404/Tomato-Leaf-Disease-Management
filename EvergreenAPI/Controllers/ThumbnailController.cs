using System;
using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Professor")]
    public class ThumbnailController : ControllerBase
    {
        private readonly IThumbnailRepository _thumbnailRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;

        public ThumbnailController(IThumbnailRepository thumbnailRepository, IMapper mapper,
            IHostingEnvironment environment, AppDbContext context)
        {
            _thumbnailRepository = thumbnailRepository;
            _mapper = mapper;
            _environment = environment;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetThumbnails()
        {
            var thumbnails = _mapper.Map<List<ThumbnailDto>>(_thumbnailRepository.GetThumbnails());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(thumbnails);
        }

        [HttpGet("{thumbnailId}")]
        [AllowAnonymous]
        public IActionResult GetThumbnail(int thumbnailId)
        {
            if (!_thumbnailRepository.ThumbnailExist(thumbnailId))
                return NotFound($"ThumbnailId '{thumbnailId}' is not exists!!");

            var thumbnail = _mapper.Map<ThumbnailDto>(_thumbnailRepository.GetThumbnail(thumbnailId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(thumbnail);
        }

        [HttpPost]
        public async Task<ActionResult> CreateThumbnail()
        {
            var postedFile = Request.Form.Files.FirstOrDefault();
            var altText = Request.Form["alt"].FirstOrDefault();

            if (postedFile == null)
                return BadRequest("File is null or empty");

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            var path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = Path.GetFileName(postedFile.FileName);
            var uniqueFilePath = Path.Combine(path, fileName);
            var uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            // Check if file name exist, use Windows style rename
            if (System.IO.File.Exists(uniqueFilePath))
            {
                var count = 1;

                var extension = Path.GetExtension(uniqueFilePath);
                var newFullPath = uniqueFilePath;

                while (System.IO.File.Exists(Path.Combine(path, newFullPath)))
                {
                    var tempFileName = $"{uniqueFileName} ({count++})";
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }

                uniqueFilePath = newFullPath;
            }

            var separatorChar = Path.DirectorySeparatorChar;
            var split = uniqueFilePath.Split(separatorChar);
            uniqueFilePath = split[^2] + "/" + split[^1];

            await using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);

            // Save thumbnail location to database
            _context.Thumbnails.Add(new Thumbnail { AltText = altText, Url = uniqueFilePath });
            await _context.SaveChangesAsync();

            var response = new HttpResponseMessage();
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With");
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateThumbnail()
        {
            var postedFile = Request.Form.Files.FirstOrDefault();
            var altText = Request.Form["alt"].FirstOrDefault();
            var thumbnailId = Request.Form["id"].FirstOrDefault();
            var oldUrl = Request.Form["old"].FirstOrDefault();

            if (postedFile == null)
                return BadRequest("File is null or empty");

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            var path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = Path.GetFileName(postedFile.FileName);
            var uniqueFilePath = Path.Combine(path, fileName);
            if (oldUrl != null)
            {
                var oldFile = Path.Combine(_environment.ContentRootPath, oldUrl);
                // Delete old file
                if (System.IO.File.Exists(oldFile))
                    System.IO.File.Delete(oldFile);
            }

            var separatorChar = Path.DirectorySeparatorChar;
            var split = uniqueFilePath.Split(separatorChar);
            uniqueFilePath = split[^2] + "/" + split[^1];

            await using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);

            // Save thumbnail location to database
            if (thumbnailId != null)
            {
                _context.Thumbnails.Update(new Thumbnail
                {
                    ThumbnailId = int.Parse(thumbnailId),
                    AltText = altText,
                    Url = uniqueFilePath
                });
            }

            await _context.SaveChangesAsync();

            var responseMessage = $"{fileName} updated successfully";
            return Ok(responseMessage);
        }

        [HttpDelete("{thumbnailId}")]
        public IActionResult DeleteThumbnail(int thumbnailId)
        {
            if (!_thumbnailRepository.ThumbnailExist(thumbnailId))
                return NotFound();

            var thumbnailToDelete = _thumbnailRepository.GetThumbnail(thumbnailId);
            var uploadPath = AppDomain.CurrentDomain.BaseDirectory;
            var thumbnailUrl = Path.Combine(uploadPath, thumbnailToDelete.Url);
            if (System.IO.File.Exists(thumbnailUrl))
                System.IO.File.Delete(thumbnailUrl);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_thumbnailRepository.DeleteThumbnail(thumbnailToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }

            return Ok("Delete Success");
        }
    }
}