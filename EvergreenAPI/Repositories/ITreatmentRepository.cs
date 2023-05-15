using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface ITreatmentRepository
    {
        ICollection<Thumbnail> GetThumbnails();
        ICollection<Treatment> GetTreatments();
        Treatment GetTreatment(int id);
        bool TreatmentExist(int id);
        bool CreateTreatment(Treatment treatment);
        bool UpdateTreatment(Treatment treatment);
        bool DeleteTreatment(Treatment treatment);
        bool Save();
        List<Treatment> Search(string search);
    }
}
