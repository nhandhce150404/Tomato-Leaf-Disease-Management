using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly AppDbContext _context;

        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateMedicine(Medicine medicine)
        {
            _context.Add(medicine);
            return Save();
        }

        public bool DeleteMedicine(Medicine medicine)
        {
            _context.Remove(medicine);
            return Save();
        }

        public bool MedicineExist(int id)
        {
            return _context.Medicines.Any(f => f.MedicineId == id);
        }

        public Medicine GetMedicine(int id)
        {

            return _context.Medicines.Include(d => d.MedicineCategory).Include(d => d.Thumbnail).FirstOrDefault(s => s.MedicineId == id);

            
        }
        public ICollection<Thumbnail> GetThumbnails()
        {
            return _context.Thumbnails.ToList();
        }

        public ICollection<MedicineCategory> GetMedicineCategories()
        {
            return _context.MedicineCategories.ToList();
        }

        public ICollection<Medicine> GetMedicines()
        {   
            var result = _context.Medicines.Include(d => d.MedicineCategory).Include(d => d.Thumbnail).ToList();
            var healthy = result.Find(d => d.Name == "Healthy leaf");
            result.Remove(healthy);
            return result;
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMedicine(Medicine medicine)
        {
            _context.Update(medicine);
            return Save();
        }



        public List<Medicine> Search(string search)
        {
            List<Medicine> d = new List<Medicine>();
            try
            {
                d = _context.Medicines.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return d;
        }


        public List<Medicine> GetMedicinesName()
        {
            var listMedicine = _context.Medicines.Include(s => s.MedicineCategory).ToList();
            return listMedicine;
        }
    }
}
