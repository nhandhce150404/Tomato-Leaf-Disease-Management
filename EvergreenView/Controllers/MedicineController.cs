using EvergreenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class MedicineController : Controller
    {
        private readonly string _thumbnailApiUrl;
        private readonly string _medicineApiUrl;
        private readonly string _medicineCategoryApiUrl;
        private readonly HttpClient _client;


        public MedicineController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _medicineApiUrl = configuration["BaseUrl"] + "/api/Medicine";
            _medicineCategoryApiUrl = configuration["BaseUrl"] + "/api/MedicineCategory";
            _thumbnailApiUrl = configuration["BaseUrl"] + "/api/Thumbnail";

        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineApiUrl);

            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(strData, options);
            return View(medicines);
        }


        public async Task<ActionResult> Details(int id)
        {
            var medicine = await GetMedicineById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }


        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }


            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory =
                JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicine medicine)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            string data = JsonSerializer.Serialize(medicine);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");


            HttpResponseMessage response = _client.PostAsync(_medicineApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Create Successfully";
                return RedirectToAction("AdminIndex");
            }

            HttpResponseMessage responeMedicineCategory = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory =
                JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
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

            HttpResponseMessage response = await _client.GetAsync(_medicineApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Medicine medicine = JsonSerializer.Deserialize<Medicine>(strData, options);


            HttpResponseMessage responeMedicineCategory = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData1 = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory =
                JsonSerializer.Deserialize<List<MedicineCategory>>(strData1, options1);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeImage1 = await _client.GetAsync(_thumbnailApiUrl);
            string strData3 = await responeImage1.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData3, options3);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            return View(medicine);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Medicine medicine)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var medicineId = await GetMedicineById(id);
            var data = JsonSerializer.Serialize(medicine);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_medicineApiUrl + "/" + id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }

            response = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listMedicineCategory = JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");

            response = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await response.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            TempData["error"] = "Can not Update";
            return View(medicineId);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var medicine = await GetMedicineById(id);
            if (medicine == null)
                return NotFound();
            await SetViewData();
            return View(medicine);
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

            var medicine = await GetMedicineById(id);
            HttpResponseMessage response = await _client.DeleteAsync(_medicineApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }
            TempData["error"] = "Can not Delete";
            return View(medicine);
        }


        private async Task<Medicine> GetMedicineById(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Medicine>(strData, options);
        }


        public async Task<IEnumerable<MedicineCategory>> GetMedicineCategories()
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory =
                JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            return listMedicineCategory;
        }


        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImage = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return listImage;
        }


        public async Task SetViewData()
        {
            var listMedicineCategory = await GetMedicineCategories();
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");
            var listImage = await GetImages();
            ViewData["Thumbnails"] = new SelectList(listImage, "ThumbnailId", "AltText");
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
                response = await _client.GetAsync(_medicineApiUrl);
            }
            else
            {
                response = await _client.GetAsync(_medicineApiUrl + query);
            }


            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(strData, options);
            return View(medicines);
        }


        public async Task<IActionResult> AdminDetails(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var medicine = await GetMedicineById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }
    }
}