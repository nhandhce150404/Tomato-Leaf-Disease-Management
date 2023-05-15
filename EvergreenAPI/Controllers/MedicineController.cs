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
    
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;

        public MedicineController(IMedicineRepository medicineRepository, IMapper mapper)
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMedicines()
        {
            var medicines = _mapper.Map<List<Medicine>>(_medicineRepository.GetMedicines());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicines);
        }

        [HttpGet("{medicineId}")]
        [AllowAnonymous]
        public IActionResult GetMedicine(int medicineId)
        {
            if (!_medicineRepository.MedicineExist(medicineId))
                return NotFound($"Medicine Category '{medicineId}' is not exists!!");

            var medicines = _mapper.Map<Medicine>(_medicineRepository.GetMedicine(medicineId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(medicines);
        }

        [HttpPost]
        public IActionResult CreateMedicine([FromBody] MedicineDto medicineCreate)
        {
            if (medicineCreate == null)
                return BadRequest(ModelState);

            var medicine = _medicineRepository
                .GetMedicines()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == medicineCreate.Name.TrimEnd().ToUpper());

            if (medicine != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineMap = _mapper.Map<Medicine>(medicineCreate);

            if (!_medicineRepository.CreateMedicine(medicineMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{medicineId}")]
        public IActionResult UpdateMedicine(int medicineId, [FromBody] MedicineDto updatedMedicine)
        {
            if (updatedMedicine == null)
                return BadRequest(ModelState);

            if (medicineId != updatedMedicine.MedicineId)
                return BadRequest(ModelState);

            if (!_medicineRepository.MedicineExist(medicineId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicineMap = _mapper.Map<Medicine>(updatedMedicine);

            if (!_medicineRepository.UpdateMedicine(medicineMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{medicineId}")]
        public IActionResult DeleteMedicine(int medicineId)
        {
            if (!_medicineRepository.MedicineExist(medicineId))
                return NotFound();

            var medicineToDelete = _medicineRepository.GetMedicine(medicineId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_medicineRepository.DeleteMedicine(medicineToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }


        [HttpGet("Search")]
        public ActionResult<List<Medicine>> Search(string search)
        {
            var list = _medicineRepository.Search(search);

            return Ok(list);
        }

        [HttpGet("GetMedicineName")]
        public ActionResult GetMedicineName()
        {
            Dictionary<string, int> amount = new Dictionary<string, int>();
            var listMedicineName = _medicineRepository.GetMedicinesName();
            var categoryName = listMedicineName.Select(c => c.MedicineCategory.Name).Distinct();
            foreach(var item in categoryName)
            {
                amount.Add(item, 0);
            }
            foreach(var item in listMedicineName.Select(l=>l.MedicineCategory))
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
