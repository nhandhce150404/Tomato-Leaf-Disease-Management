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
    [Authorize(Roles = "Admin")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ImageController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetImages()
        {
            var images = _mapper.Map<List<ImageDto>>(_imageRepository.GetImages());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(images);
        }

        [HttpGet("{imageId}")]
        [AllowAnonymous]
        public IActionResult GetImage(int imageId)
        {
            if (!_imageRepository.ImageExist(imageId))
                return NotFound($"ImageId '{imageId}' is not exists!!");

            var image = _mapper.Map<ImageDto>(_imageRepository.GetImage(imageId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(image);
        }

        [HttpPost]
        public IActionResult CreateImage([FromBody] ImageDto imageCreate)
        {
            if (imageCreate == null)
                return BadRequest(ModelState);

            var image = _imageRepository
                .GetImages()
                .FirstOrDefault(c => c.AltText.Trim().ToUpper() == imageCreate.AltText.TrimEnd().ToUpper());

            if (image != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageMap = _mapper.Map<Image>(imageCreate);

            if (!_imageRepository.CreateImage(imageMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{imageId}")]
        public IActionResult UpdateImage(int imageId, [FromBody] ImageDto updatedImage)
        {
            if (updatedImage == null)
                return BadRequest(ModelState);

            if (imageId != updatedImage.ImageId)
                return BadRequest(ModelState);

            if (!_imageRepository.ImageExist(imageId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageMap = _mapper.Map<Image>(updatedImage);

            if (!_imageRepository.UpdateImage(imageMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{imageId}")]
        public IActionResult DeleteImage(int imageId)
        {
            if (!_imageRepository.ImageExist(imageId))
                return NotFound();

            var imageToDelete = _imageRepository.GetImage(imageId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_imageRepository.DeleteImage(imageToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
