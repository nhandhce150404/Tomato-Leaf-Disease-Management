using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EvergreenView.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _medicineApiUrl;
        private readonly string _diseaseApiUrl;
        private readonly HttpClient _client;

        public AdminController(IConfiguration configuration)
        {
            var baseUrl = configuration["BaseUrl"];

            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _medicineApiUrl = configuration["BaseUrl"] + "/api/Medicine";
            _diseaseApiUrl = configuration["BaseUrl"] + "/api/Disease";
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("t");
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Medi Chart
            HttpResponseMessage response = await _client.GetAsync(_medicineApiUrl + "/GetMedicineName");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var dictionaryObject = JsonSerializer.Deserialize<Dictionary<string, int>>(strData, options);
            var keys = new List<string>();
            var values = new List<int>();
            foreach (var item in dictionaryObject)
            {
                keys.Add(item.Key);
                values.Add(item.Value);
            }

            var jsonLabel = JsonSerializer.Serialize(keys);
            var jsonData = JsonSerializer.Serialize(values);
            HttpContext.Session.SetString("labels", jsonLabel);
            HttpContext.Session.SetString("datas", jsonData);

            //Disease Chart
            HttpResponseMessage response1 = await _client.GetAsync(_diseaseApiUrl + "/GetDiseaseName");
            string strData1 = await response1.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var dictionaryObject1 = JsonSerializer.Deserialize<Dictionary<string, int>>(strData1, options1);
            var keysDisease = new List<string>();
            var valuesDisease = new List<int>();
            foreach (var item1 in dictionaryObject1)
            {
                keysDisease.Add(item1.Key);
                valuesDisease.Add(item1.Value);
            }

            var jsonLabelDisease = JsonSerializer.Serialize(keysDisease);
            var jsonDataDisease = JsonSerializer.Serialize(valuesDisease);
            HttpContext.Session.SetString("labelDisease", jsonLabelDisease);
            HttpContext.Session.SetString("dataDisease", jsonDataDisease);

            return View();
        }
    }
}