using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IMedicineCategoryRepository
    {
        ICollection<MedicineCategory> GetMedicineCategories();
        MedicineCategory GetMedicineCategory(int id);
        bool MedicineCategoryExist(int id);
        bool CreateMedicineCategory(MedicineCategory medicineCategory);
        bool UpdateMedicineCategory(MedicineCategory medicineCategory);
        bool DeleteMedicineCategory(MedicineCategory medicineCategory);
        bool Save();
        List<string> GetMedicineCategoryName();
    }
}
