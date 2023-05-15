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
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public TreatmentController(ITreatmentRepository treatmentRepository, IMapper mapper)
        {
            _treatmentRepository = treatmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetTreatments()
        {
            var treatments = _mapper.Map<List<Treatment>>(_treatmentRepository.GetTreatments());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(treatments);
        }

        [HttpGet("{treatmentId}")]
        [AllowAnonymous]
        public IActionResult GetTreatment(int treatmentId)
        {
            if (!_treatmentRepository.TreatmentExist(treatmentId))
                return NotFound($"Treatment '{treatmentId}' is not exists!!");

            var treatment = _mapper.Map<Treatment>(_treatmentRepository.GetTreatment(treatmentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(treatment);
        }

        [HttpPost]
        public IActionResult CreateTreatment([FromBody] TreatmentDto treatmentCreate)
        {
            if (treatmentCreate == null)
                return BadRequest(ModelState);

            var plant = _treatmentRepository
                .GetTreatments()
                .FirstOrDefault(c => c.TreatmentName.Trim().ToUpper() == treatmentCreate.TreatmentName.TrimEnd().ToUpper());

            if (plant != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var treatmentMap = _mapper.Map<Treatment>(treatmentCreate);

            if (!_treatmentRepository.CreateTreatment(treatmentMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }


        [HttpPut("{treatmentId}")]
        public IActionResult UpdateTreatment(int treatmentId, [FromBody] TreatmentDto updatedTreatment)
        {
            if (updatedTreatment == null)
                return BadRequest(ModelState);

            if (treatmentId != updatedTreatment.TreatmentId)
                return BadRequest(ModelState);

            if (!_treatmentRepository.TreatmentExist(treatmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var treatmentMap = _mapper.Map<Treatment>(updatedTreatment);

            if (!_treatmentRepository.UpdateTreatment(treatmentMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }


        [HttpDelete("{treatmentId}")]
        public IActionResult DeletePlantCategory(int treatmentId)
        {
            if (!_treatmentRepository.TreatmentExist(treatmentId))
                return NotFound();

            var treatmentToDelete = _treatmentRepository.GetTreatment(treatmentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_treatmentRepository.DeleteTreatment(treatmentToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }

            return Ok("Delete Success");
        }


        [HttpGet("Search")]
        public ActionResult<List<Treatment>> Search(string search)
        {
            var list = _treatmentRepository.Search(search);

            return Ok(list);
        }
    }
}