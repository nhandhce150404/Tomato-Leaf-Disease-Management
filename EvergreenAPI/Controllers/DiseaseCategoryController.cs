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
    [Authorize(Roles = "Admin, Professor")]
    public class DiseaseCategoryController : ControllerBase
    {
        private readonly IDiseaseCategoryRepository _diseaseCategoryRepository;
        private readonly IMapper _mapper;

        public DiseaseCategoryController(IDiseaseCategoryRepository diseaseCategoryRepository, IMapper mapper)
        {
            _diseaseCategoryRepository = diseaseCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetDiseaseCategories()
        {
            var diseaseCategories = _mapper.Map<List<DiseaseCategoryDto>>(_diseaseCategoryRepository.GetDiseaseCategories());

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(diseaseCategories);
        }

        [HttpGet("{diseaseCategoryId}")]
        [AllowAnonymous]
        public IActionResult GetDiseaseCategory(int diseaseCategoryId)
        {
            if (!_diseaseCategoryRepository.DiseaseCategoryExist(diseaseCategoryId)) 
                return NotFound($"Disease Category '{diseaseCategoryId}' is not exists!!");

            var diseaseCategory = _mapper.Map<DiseaseCategoryDto>(_diseaseCategoryRepository.GetDiseaseCategory(diseaseCategoryId));

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(diseaseCategory);
        }

        [HttpPost]
        public IActionResult CreateDiseaseCategory([FromBody] DiseaseCategoryDto diseaseCateCreate)
        {
            if (diseaseCateCreate == null)
                return BadRequest(ModelState);

            var diseaseCate = _diseaseCategoryRepository
                .GetDiseaseCategories()
                .FirstOrDefault(c => string.Equals(c.Name.Trim(), diseaseCateCreate.Name.TrimEnd(), StringComparison.CurrentCultureIgnoreCase));

            if (diseaseCate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diseaseCategoryMap = _mapper.Map<DiseaseCategory>(diseaseCateCreate);

            if (!_diseaseCategoryRepository.CreateDiseaseCategory(diseaseCategoryMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{diseaseCategoryId}")]
        public IActionResult UpdateDiseaseCategory(int diseaseCategoryId, [FromBody] DiseaseCategoryDto updatedDiseaseCategory)
        {
            if (updatedDiseaseCategory == null) 
                return BadRequest(ModelState);

            if (diseaseCategoryId != updatedDiseaseCategory.DiseaseCategoryId) 
                return BadRequest(ModelState);

            if (!_diseaseCategoryRepository.DiseaseCategoryExist(diseaseCategoryId)) 
                return NotFound();

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var diseaseCategoryMap = _mapper.Map<DiseaseCategory>(updatedDiseaseCategory);

            if (!_diseaseCategoryRepository.UpdateDiseaseCategory(diseaseCategoryMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }



        [HttpDelete("{diseaseCategoryId}")]
        public IActionResult DeleteDiseaseCategory(int diseaseCategoryId)
        {
            if (!_diseaseCategoryRepository.DiseaseCategoryExist(diseaseCategoryId)) 
                return NotFound();

            var diseaseCateToDelete = _diseaseCategoryRepository.GetDiseaseCategory(diseaseCategoryId);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (!_diseaseCategoryRepository.DeleteDiseaseCategory(diseaseCateToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
