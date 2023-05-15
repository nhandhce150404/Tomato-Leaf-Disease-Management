using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class DiseaseCategoryRepository : IDiseaseCategoryRepository
    {
        private readonly AppDbContext _context;

        public DiseaseCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateDiseaseCategory(DiseaseCategory diseaseCategory)
        {
            _context.Add(diseaseCategory);
            return Save();
        }

        public bool DeleteDiseaseCategory(DiseaseCategory diseaseCategory)
        {
            _context.Remove(diseaseCategory);
            return Save();
        }

        public bool DiseaseCategoryExist(int id)
        {
            return _context.DiseaseCategories.Any(f => f.DiseaseCategoryId == id);
        }

        public DiseaseCategory GetDiseaseCategory(int id)
        {
            return _context.DiseaseCategories.FirstOrDefault(s => s.DiseaseCategoryId == id);
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

        public bool UpdateDiseaseCategory(DiseaseCategory diseaseCategory)
        {
            _context.Update(diseaseCategory);
            return Save();
        }
    }
}
