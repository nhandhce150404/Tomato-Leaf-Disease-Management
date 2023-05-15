using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly AppDbContext _context;

        public DiseaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateDisease(Disease disease)
        {
            _context.Add(disease);
            return Save();
        }

        public bool DeleteDisease(Disease disease)
        {
            _context.Remove(disease);
            return Save();
        }

        public bool DiseaseExist(int id)
        {
            return _context.Diseases.Any(f => f.DiseaseId == id);
        }

        public Disease GetDisease(int id)
        {
            return _context.Diseases
                .Include(d => d.DiseaseCategory)
                .Include(d => d.Medicine)
                .Include(d => d.Treatment)
                .Include(d => d.Thumbnail).FirstOrDefault(s => s.DiseaseId == id); ;
        }

        public ICollection<Disease> GetDiseases()
        {
            var result = _context.Diseases
                .Include(d => d.DiseaseCategory)
                .Include(d => d.Medicine)
                .Include(d => d.Treatment)
                .Include(d => d.Thumbnail).ToList();

            var healthy = result.Find(d => d.Name == "Healthy Leaf");
            result.Remove(healthy);

            return result;
        }

        public ICollection<Thumbnail> GetThumbnails()
        {
            return _context.Thumbnails.ToList();
        }

        public ICollection<Medicine> GetMedicines()
        {
            return _context.Medicines.ToList();
        }


        public ICollection<Treatment> GetTreatments()
        {
            return _context.Treatments.ToList();
        }

        public ICollection<DiseaseCategory> GetDiseaseCategories()
        {
            return _context.DiseaseCategories.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateDisease(Disease disease)
        {
            _context.Update(disease);
            return Save();
        }




        public List<Disease> Search(string search)
        {
            List<Disease> d = new List<Disease>();
            try
            {
                d = _context.Diseases.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return d;
        }

        public List<Disease> GetDiseasesName()
        {
            var listDisease = _context.Diseases.Include(s => s.DiseaseCategory).ToList();
            return listDisease;
        }
    }
}
