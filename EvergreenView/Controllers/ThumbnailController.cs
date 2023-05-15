using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class ThumbnailController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _thumbnailApiUrl;

        public ThumbnailController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _thumbnailApiUrl = configuration["BaseUrl"] + "/api/Thumbnail";

        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
                return RedirectToAction("Index", "Home");

            var response = await _client.GetAsync(_thumbnailApiUrl);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return View(listImages);
        }

        public IActionResult Create()
        {
            ViewData["t"] = HttpContext.Session.GetString("t");
            return View();
        }

        public async Task<ActionResult> Update(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
                return RedirectToAction("Index");

            var thumbnail = await GetThumbnailAsync(id);
            if (thumbnail == null) return NotFound();

            ViewData["t"] = HttpContext.Session.GetString("t");
            return View(thumbnail);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
                return RedirectToAction("Index");

            var thumbnail = await GetThumbnailAsync(id);
            if (thumbnail == null) return NotFound();
            return View(thumbnail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Thumbnail thumbnail)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
                return RedirectToAction("Index");

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (thumbnail == null) return NotFound();

            HttpResponseMessage response = await _client.DeleteAsync(_thumbnailApiUrl + "/" + thumbnail.ThumbnailId);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return RedirectToAction("Delete", thumbnail.ThumbnailId);
        }

        private async Task<Thumbnail> GetThumbnailAsync(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
                return null;

            HttpResponseMessage response = await _client.GetAsync(_thumbnailApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var thumbnail = JsonSerializer.Deserialize<Thumbnail>(strData, options);
            return thumbnail;
        }
    }
}