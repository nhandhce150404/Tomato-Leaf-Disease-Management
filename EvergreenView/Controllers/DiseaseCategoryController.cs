using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class DiseaseCategoryController : Controller
    {
        private readonly string _diseaseCategoryApiUrl;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DiseaseCategoryController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _diseaseCategoryApiUrl = configuration["BaseUrl"] + "/api/DiseaseCategory";

            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session
        {
            get { return _httpContextAccessor.HttpContext?.Session; }
        }


        public async Task<IActionResult> Index()
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            HttpResponseMessage response = await _client.GetAsync(_diseaseCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<DiseaseCategory> diseaseCategories =
                JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            return View(diseaseCategories);
        }


        public async Task<ActionResult> Details(int id)
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var member = await GetDiseaseCategoryById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }


        public ActionResult Create()
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DiseaseCategory diseaseCategory)
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(diseaseCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_diseaseCategoryApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Create Successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Can not Create";
            return View();
        }


        public async Task<ActionResult> Edit(int id)
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            HttpResponseMessage response = await _client.GetAsync(_diseaseCategoryApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            DiseaseCategory diseaseCategory = JsonSerializer.Deserialize<DiseaseCategory>(strData, options);

            return View(diseaseCategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiseaseCategory diseaseCategory)
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var diseaseCat = await GetDiseaseCategoryById(id);
            string data = JsonSerializer.Serialize(diseaseCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_diseaseCategoryApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Can not Update";
            return View(diseaseCat);
        }

        private async Task<DiseaseCategory> GetDiseaseCategoryById(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_diseaseCategoryApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<DiseaseCategory>(strData, options);
        }


        public async Task<ActionResult> Delete(int id)
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var member = await GetDiseaseCategoryById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Session.GetString("r") != "Admin" && Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var disease = await GetDiseaseCategoryById(id);
            HttpResponseMessage response = await _client.DeleteAsync(_diseaseCategoryApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Can not Delete";
            return View(disease);
        }
    }
}