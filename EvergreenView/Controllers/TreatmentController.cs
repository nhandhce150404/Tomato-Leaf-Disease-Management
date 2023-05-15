using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly string _thumbnailApiUrl;
        private readonly string _treatmentApiUrl;
        private readonly HttpClient _client;

        public TreatmentController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");

            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _treatmentApiUrl = configuration["BaseUrl"] + "/api/Treatment";
            _thumbnailApiUrl = configuration["BaseUrl"] + "/api/Thumbnail";

        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_treatmentApiUrl);

            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Treatment> treatments = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return View(treatments);
        }


        public async Task<ActionResult> Details(int id)
        {
            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            return View(treatment);
        }


        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }


            HttpResponseMessage responseImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData1 = await responseImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData1, options1);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Treatment treatment)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(treatment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_treatmentApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Create Successfully";
                return RedirectToAction("AdminIndex");
            }


            HttpResponseMessage responseImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData1 = await responseImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData1, options1);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            TempData["error"] = "Can not Create";
            return View();
        }


        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await _client.GetAsync(_treatmentApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Treatment treatment = JsonSerializer.Deserialize<Treatment>(strData, options);


            HttpResponseMessage responseImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await responseImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View(treatment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Treatment treatment)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var treatmentId = await GetTreatmentById(id);
            var data = JsonSerializer.Serialize(treatment);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_treatmentApiUrl + "/" + id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }

            response = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await response.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            TempData["error"] = "Can not Update";
            return View(treatmentId);
        }


        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            await SetViewData();
            return View(treatment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var treatment = await GetTreatmentById(id);
            HttpResponseMessage response = await _client.DeleteAsync(_treatmentApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }

            TempData["error"] = "Can not Delete";
            return View(treatment);
        }

        private async Task<Treatment> GetTreatmentById(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_treatmentApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Treatment>(strData, options);
        }


        public async Task SetViewData()
        {
            var listImage = await GetImages();
            ViewData["Thumbnails"] = new SelectList(listImage, "ThumbnailId", "AltText");
        }


        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await _client.GetAsync(_treatmentApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImage = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return listImage;
        }


        public async Task<IActionResult> AdminIndex(string searchString)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }


            string query = null;
            if (searchString != null)
                query = "/Search" + "?search=" + searchString;


            HttpResponseMessage response;
            if (query == null)
            {
                response = await _client.GetAsync(_treatmentApiUrl);
            }
            else
            {
                response = await _client.GetAsync(_treatmentApiUrl + query);
            }


            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Treatment> treatments = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return View(treatments);
        }


        public async Task<IActionResult> AdminDetails(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            return View(treatment);
        }
    }
}