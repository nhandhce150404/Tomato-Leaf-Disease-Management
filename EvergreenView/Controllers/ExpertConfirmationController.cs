using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EvergreenAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class ExpertConfirmationController : Controller
    {
        private readonly string _detectionHistoryApiUrl;
        private readonly HttpClient _client;

        public ExpertConfirmationController(IConfiguration configuration)
        {

            _detectionHistoryApiUrl = configuration["BaseUrl"] + "/api/DetectionHistory";

            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        // GET
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("r");
            if (role is null or "User")
                return RedirectToAction("Index", "Home");

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync(_detectionHistoryApiUrl);
            var strData = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");

            var histories = JsonSerializer.Deserialize<List<ExtractDetectionHistoriesDto>>(strData);

            return View(histories);
        }
    }
}