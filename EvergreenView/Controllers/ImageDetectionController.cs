using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class ImageDetectionController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _detectionApiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ImageDetectionController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);


            _detectionApiUrl = configuration["BaseUrl"] + "/api/DetectionHistory";

            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session
        {
            get { return _httpContextAccessor.HttpContext?.Session; }
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("r")))
                return RedirectToAction("Login", "Authentication");

            var query = "/" + Session.GetString("i");
            var response = await _client.GetAsync(_detectionApiUrl + query);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var history = JsonSerializer.Deserialize<List<ExtractDetectionHistoriesDto>>(strData, options);
            var result = JsonSerializer.Serialize(history);
            ViewBag.history = result;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("r")))
            //    return RedirectToAction("Login", "Authentication");

            var query = "/Details/" + id;
            var response = await _client.GetAsync(_detectionApiUrl + query);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var accuracies = JsonSerializer.Deserialize<List<DetectionAccuracy>>(strData, options);
            return View(accuracies);
        }
    }
}