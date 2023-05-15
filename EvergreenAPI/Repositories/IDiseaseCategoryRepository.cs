using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IDiseaseCategoryRepository
    {
        ICollection<DiseaseCategory> GetDiseaseCategories();
        DiseaseCategory GetDiseaseCategory(int id);
        bool DiseaseCategoryExist(int id);
        bool CreateDiseaseCategory(DiseaseCategory diseaseCategory);
        bool UpdateDiseaseCategory(DiseaseCategory diseaseCategory);
        bool DeleteDiseaseCategory(DiseaseCategory diseaseCategory);
        bool Save();
    }
}
