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
    public class DiseaseController : Controller
    {
        private readonly string _diseaseApiUrl;
        private readonly string _diseaseCategoryApiUrl;
        private readonly string _thumbnailApiUrl;
        private readonly string _medicineApiUrl;
        private readonly string _treatmentApiUrl;
        private readonly HttpClient _client;

        public DiseaseController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);



            _diseaseApiUrl = configuration["BaseUrl"] + "/api/Disease";
            _diseaseCategoryApiUrl = configuration["BaseUrl"] + "/api/DiseaseCategory";
            _thumbnailApiUrl = configuration["BaseUrl"] + "/api/Thumbnail";
            _medicineApiUrl = configuration["BaseUrl"] + "/api/Medicine";
            _treatmentApiUrl = configuration["BaseUrl"] + "/api/Treatment";

        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(_diseaseApiUrl);

            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Disease> diseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);

            return View(diseases);
        }

        public async Task<ActionResult> Details(int id)
        {
            var disease = await GetDiseaseById(id);
            if (disease == null)
                return NotFound();
            return View(disease);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var responseDiseaseCategory = await _client.GetAsync(_diseaseCategoryApiUrl);
            var strData = await responseDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            var responseImage = await _client.GetAsync(_thumbnailApiUrl);
            var strData1 = await responseImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData1, options1);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");


            var responseMedicine = await _client.GetAsync(_medicineApiUrl);
            var strData2 = await responseMedicine.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData2, options2);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");


            var responseTreatment = await _client.GetAsync(_treatmentApiUrl);
            var strData3 = await responseTreatment.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData3, options3);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "TreatmentName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disease disease)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(disease);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_diseaseApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Create Successfully";
                return RedirectToAction("AdminIndex");
            }

            HttpResponseMessage responseDiseaseCategory = await _client.GetAsync(_diseaseCategoryApiUrl);
            string strData = await responseDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory =
                JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responseImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData1 = await responseImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData1, options1);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");


            HttpResponseMessage responseMedicine = await _client.GetAsync(_medicineApiUrl);
            string strData2 = await responseMedicine.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData2, options2);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");


            HttpResponseMessage responseTreatment = await _client.GetAsync(_treatmentApiUrl);
            string strData3 = await responseTreatment.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData3, options3);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "TreatmentName");
            TempData["error"] = "Can not Create";
            return View();
        }


        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await _client.GetAsync(_diseaseApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Disease disease = JsonSerializer.Deserialize<Disease>(strData, options);

            HttpResponseMessage responseDiseaseCategory = await _client.GetAsync(_diseaseCategoryApiUrl);
            string strData1 = await responseDiseaseCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory =
                JsonSerializer.Deserialize<List<DiseaseCategory>>(strData1, options1);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responseImage = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await responseImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");


            HttpResponseMessage responseMedicine = await _client.GetAsync(_medicineApiUrl);
            string strData3 = await responseMedicine.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData3, options3);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");


            HttpResponseMessage responseTreatment = await _client.GetAsync(_treatmentApiUrl);
            string strData4 = await responseTreatment.Content.ReadAsStringAsync();
            var options4 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData4, options4);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "TreatmentName");


            return View(disease);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Disease disease)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var diseaseId = await GetDiseaseById(id);
            var data = JsonSerializer.Serialize(disease);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_diseaseApiUrl + "/" + id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }

            response = await _client.GetAsync(_diseaseCategoryApiUrl);
            string strData1 = await response.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory =
                JsonSerializer.Deserialize<List<DiseaseCategory>>(strData1, options1);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            response = await _client.GetAsync(_thumbnailApiUrl);
            string strData2 = await response.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");


            response = await _client.GetAsync(_medicineApiUrl);
            string strData3 = await response.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData3, options3);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");


            response = await _client.GetAsync(_treatmentApiUrl);
            string strData4 = await response.Content.ReadAsStringAsync();
            var options4 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData4, options4);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "TreatmentName");

            TempData["error"] = "Can not Update";
            return View(diseaseId);
        }


        private async Task<Disease> GetDiseaseById(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_diseaseApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Disease>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var disease = await GetDiseaseById(id);
            if (disease == null)
                return NotFound();
            await SetViewData();
            return View(disease);
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

            var disease = await GetDiseaseById(id);
            HttpResponseMessage response = await _client.DeleteAsync(_diseaseApiUrl + "/" + id);

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }
            TempData["error"] = "Can not Delete";
            return View(disease);
        }

        public async Task<IEnumerable<DiseaseCategory>> GetDiseaseCategory()
        {
            HttpResponseMessage response = await _client.GetAsync(_diseaseCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory =
                JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            return listDiseaseCategory;
        }


        public async Task<IEnumerable<Medicine>> GetMedicine()
        {
            HttpResponseMessage response = await _client.GetAsync(_medicineApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData, options);
            return listMedicine;
        }


        public async Task<IEnumerable<Treatment>> GetTreatment()
        {
            HttpResponseMessage response = await _client.GetAsync(_treatmentApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return listTreatment;
        }


        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await _client.GetAsync(_thumbnailApiUrl);
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
            var listTreatment = await GetTreatment();
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "TreatmentName");
            var listMedicine = await GetMedicine();
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");
            var listDiseaseCategory = await GetDiseaseCategory();
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");
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
                response = await _client.GetAsync(_diseaseApiUrl);
            }
            else
            {
                response = await _client.GetAsync(_diseaseApiUrl + query);
            }

            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Disease> diseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);

            return View(diseases);
        }


        public async Task<IActionResult> AdminDetails(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var disease = await GetDiseaseById(id);
            if (disease == null)
                return NotFound();
            return View(disease);
        }
    }
}