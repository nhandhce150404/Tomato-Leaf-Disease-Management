using System.Linq;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Professor")]
    public class ExpertConfirmationController : Controller
    {
        private readonly AppDbContext _context;

        public ExpertConfirmationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int detectionHistoryId, Disease disease)
        {
            var detectionHistory =
                _context.DetectionHistories.FirstOrDefault(d => d.DetectionHistoryId == detectionHistoryId);
            if (detectionHistory == null) return NotFound();

            detectionHistory.IsExpertConfirmed = true;
            detectionHistory.DetectedDisease = disease;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}