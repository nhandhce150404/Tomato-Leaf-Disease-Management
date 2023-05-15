using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IDiseaseRepository
    {
        ICollection<Disease> GetDiseases();
        ICollection<DiseaseCategory> GetDiseaseCategories();
        ICollection<Thumbnail> GetThumbnails();
        ICollection<Medicine> GetMedicines();
        ICollection<Treatment> GetTreatments();
        Disease GetDisease(int id);
        bool DiseaseExist(int id);
        bool CreateDisease(Disease disease);
        bool UpdateDisease(Disease disease);
        bool DeleteDisease(Disease disease);
        bool Save();
        List <Disease> Search (string search);
        List<Disease> GetDiseasesName();
    }
}
