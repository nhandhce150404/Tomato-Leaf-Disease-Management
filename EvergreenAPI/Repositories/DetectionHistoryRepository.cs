using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class DetectionHistoryRepository : IDetectionHistoryRepository
    {
        private readonly AppDbContext _context;

        public DetectionHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public DetectionHistory GetDetectionHistory(int id)
        {
            return _context.DetectionHistories.FirstOrDefault(s => s.DetectionHistoryId == id);
        }

        public ICollection<DetectionHistory> GetDetectionHistories(int accountId)
        {
            return _context.DetectionHistories.Where(d => d.AccountId == accountId).ToList();
        }
        
        public ICollection<DetectionHistory> GetAll()
        {
            return _context.DetectionHistories.ToList();
        }

        public bool Exist(int id) => _context.DetectionHistories.Any(f => f.DetectionHistoryId == id);
    }
}
