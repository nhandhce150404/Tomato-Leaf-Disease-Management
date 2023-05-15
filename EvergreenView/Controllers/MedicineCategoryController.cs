using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class MedicineCategoryController : Controller
    {
        private readonly string _medicineCategoryApiUrl;
        private readonly HttpClient _client;

        public MedicineCategoryController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _medicineCategoryApiUrl = configuration["BaseUrl"] + "/api/MedicineCategory";

        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<MedicineCategory> medicineCategories =
                JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            return View(medicineCategories);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var medicine = await GetMedicineCategoryById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }

        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DiseaseCategory diseaseCategory)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(diseaseCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_medicineCategoryApiUrl, content).Result;
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
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MedicineCategory medicineCategory = JsonSerializer.Deserialize<MedicineCategory>(strData, options);

            return View(medicineCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, MedicineCategory medicineCategory)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var mediCat = await GetMedicineCategoryById(id);
            string data = JsonSerializer.Serialize(medicineCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_medicineCategoryApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Can not Update";
            return View(mediCat);
        }

        private async Task<MedicineCategory> GetMedicineCategoryById(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<MedicineCategory>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var medicine = await GetMedicineCategoryById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var medicine = await GetMedicineCategoryById(id);
            HttpResponseMessage response = await _client.DeleteAsync(_medicineCategoryApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Can not Delete";
            return View(medicine);
        }
    }
}