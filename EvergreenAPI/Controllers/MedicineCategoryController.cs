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
    [Authorize (Roles = "Admin, Professor")]
    public class MedicineCategoryController : ControllerBase
    {
        private readonly IMedicineCategoryRepository _medicineCategoryRepository;
        private readonly IMapper _mapper;

        public MedicineCategoryController(IMedicineCategoryRepository medicineCategoryRepository, IMapper mapper)
        {
            _medicineCategoryRepository = medicineCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMedicineCategories()
        {
            var medicineCategories = _mapper.Map<List<MedicineCategoryDto>>(_medicineCategoryRepository.GetMedicineCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicineCategories);
        }

        [HttpGet("{medicineCategoryId}")]
        [AllowAnonymous]
        public IActionResult GetMedicineCategory(int medicineCategoryId)
        {
            if (!_medicineCategoryRepository.MedicineCategoryExist(medicineCategoryId))
                return NotFound($"Medicine Category '{medicineCategoryId}' is not exists!!");

            var medicineCategory = _mapper.Map<MedicineCategoryDto>(_medicineCategoryRepository.GetMedicineCategory(medicineCategoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicineCategory);
        }

        [HttpPost]
        public IActionResult CreateMedicineCategory([FromBody] MedicineCategoryDto medicineCateCreate)
        {
            if (medicineCateCreate == null)
                return BadRequest(ModelState);

            var medicineCate = _medicineCategoryRepository
                .GetMedicineCategories()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == medicineCateCreate.Name.TrimEnd().ToUpper());

            if (medicineCate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineCateMap = _mapper.Map<MedicineCategory>(medicineCateCreate);

            if (!_medicineCategoryRepository.CreateMedicineCategory(medicineCateMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{medicineCategoryId}")]
        public IActionResult UpdateMedicineCategory(int medicineCategoryId, [FromBody] MedicineCategoryDto updatedMedicineCate)
        {
            if (updatedMedicineCate == null)
                return BadRequest(ModelState);

            if (medicineCategoryId != updatedMedicineCate.MedicineCategoryId)
                return BadRequest(ModelState);

            if (!_medicineCategoryRepository.MedicineCategoryExist(medicineCategoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineCateMap = _mapper.Map<MedicineCategory>(updatedMedicineCate);

            if (!_medicineCategoryRepository.UpdateMedicineCategory(medicineCateMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{medicineCategoryId}")]
        public IActionResult DeleteMedicineCategory(int medicineCategoryId)
        {
            if (!_medicineCategoryRepository.MedicineCategoryExist(medicineCategoryId))
                return NotFound();

            var medicineCateToDelete = _medicineCategoryRepository.GetMedicineCategory(medicineCategoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_medicineCategoryRepository.DeleteMedicineCategory(medicineCateToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }

       [HttpGet("GetMedicineCategoryName")]
       public ActionResult GetMedicineCategoryName()
        {
            var listCategoriesName = _medicineCategoryRepository.GetMedicineCategoryName();
            return Ok(new
            {
                listCategoriesName
            });
        }
    }
}
