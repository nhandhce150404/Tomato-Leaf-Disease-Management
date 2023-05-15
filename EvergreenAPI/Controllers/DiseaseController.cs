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
    
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMapper _mapper;

        public DiseaseController(IDiseaseRepository diseaseRepository, IMapper mapper)
        {
            _diseaseRepository = diseaseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetDiseases()
        {
            var diseases = _mapper.Map<List<Disease>>(_diseaseRepository.GetDiseases());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(diseases);
        }

        [HttpGet("{diseaseId}")]
        [AllowAnonymous]
        public IActionResult GetDisease(int diseaseId)
        {
            if (!_diseaseRepository.DiseaseExist(diseaseId))
                return NotFound($"Disease '{diseaseId}' is not exists!!");

            var disease = _mapper.Map<Disease>(_diseaseRepository.GetDisease(diseaseId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(disease);
        }

        [HttpPost]
        public IActionResult CreateDisease([FromBody] DiseaseDto diseaseCreate)
        {
            if (diseaseCreate == null)
                return BadRequest(ModelState);

            var disease = _diseaseRepository
                .GetDiseases()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == diseaseCreate.Name.TrimEnd().ToUpper());

            if (disease != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diseaseMap = _mapper.Map<Disease>(diseaseCreate);

            if (!_diseaseRepository.CreateDisease(diseaseMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{diseaseId}")]
        public IActionResult UpdateDisease(int diseaseId, [FromBody] DiseaseDto updatedDisease)
        {
            if (updatedDisease == null)
                return BadRequest(ModelState);

            if (diseaseId != updatedDisease.DiseaseId)
                return BadRequest(ModelState);

            if (!_diseaseRepository.DiseaseExist(diseaseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diseaseMap = _mapper.Map<Disease>(updatedDisease);

            if (!_diseaseRepository.UpdateDisease(diseaseMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{diseaseId}")]
        public IActionResult DeleteDisease(int diseaseId)
        {
            if (!_diseaseRepository.DiseaseExist(diseaseId))
                return NotFound();

            var diseaseToDelete = _diseaseRepository.GetDisease(diseaseId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_diseaseRepository.DeleteDisease(diseaseToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }

        [HttpGet("Search")]
        public ActionResult<List<Disease>> Search(string search)
        {
            var list = _diseaseRepository.Search(search);
            
            return Ok(list);
        }

        [HttpGet("GetDiseaseName")]
        public ActionResult GetMedicineName()
        {
            Dictionary<string, int> amount = new Dictionary<string, int>();
            var listDiseaseName = _diseaseRepository.GetDiseasesName();
            var categoryName = listDiseaseName.Select(c => c.DiseaseCategory.Name).Distinct();
            foreach (var item in categoryName)
            {
                amount.Add(item, 0);
            }
            foreach (var item in listDiseaseName.Select(l => l.DiseaseCategory))
            {
                if (amount.ContainsKey(item.Name))
                {
                    amount[item.Name]++;
                }
            }

            return Ok(amount);
        }
    }
}
