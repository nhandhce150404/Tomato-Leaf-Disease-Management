using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IDetectionHistoryRepository
    {
        ICollection<DetectionHistory> GetDetectionHistories(int accountId);
        ICollection<DetectionHistory> GetAll();
        DetectionHistory GetDetectionHistory(int id);
        bool Exist(int id);
    }
}
