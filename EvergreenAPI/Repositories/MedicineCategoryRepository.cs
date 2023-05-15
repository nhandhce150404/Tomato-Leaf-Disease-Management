using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class MedicineCategoryRepository : IMedicineCategoryRepository
    {
        private readonly AppDbContext _context;

        public MedicineCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateMedicineCategory(MedicineCategory medicineCategory)
        {
            _context.Add(medicineCategory);
            return Save();
        }

        public bool DeleteMedicineCategory(MedicineCategory medicineCategory)
        {
            _context.Remove(medicineCategory);
            return Save();
        }

        public bool MedicineCategoryExist(int id)
        {
            return _context.MedicineCategories.Any(f => f.MedicineCategoryId == id);
        }

        public MedicineCategory GetMedicineCategory(int id)
        {
            return _context.MedicineCategories.FirstOrDefault(s => s.MedicineCategoryId == id);
        }

        public ICollection<MedicineCategory> GetMedicineCategories()
        {
            return _context.MedicineCategories.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMedicineCategory(MedicineCategory medicineCategory)
        {
            _context.Update(medicineCategory);
            return Save();
        }

        public List<string> GetMedicineCategoryName()
        {
            return _context.MedicineCategories.Select(s => s.Name).ToList();
        }
    }
}
