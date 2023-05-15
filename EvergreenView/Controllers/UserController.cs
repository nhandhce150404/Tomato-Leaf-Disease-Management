using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using EvergreenAPI.DTO;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _userApiUrl;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);

            _configuration = configuration;
            _userApiUrl = _configuration["BaseUrl"] + "/api/User";
        }

        public async Task<ActionResult> AdminDetails(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_userApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            var user = new Account()
            {
                AccountId = id,
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                FullName = (string)temp["fullName"],
                Role = (string)temp["role"],
                AvatarUrl = (string)temp["avatarUrl"],
                VerifiedAt = DateTime.Parse((string)temp["verifiedAt"] ?? string.Empty),
                Professions = (string)temp["professions"]
            };

            return View(user);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!IsCurrentUser(id)) return BadRequest();

            var token = HttpContext.Session.GetString("t");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_userApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            var user = new Account()
            {
                AccountId = id,
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                FullName = (string)temp["fullName"],
                AvatarUrl = (string)temp["avatarUrl"],
                VerifiedAt = DateTime.Parse((string)temp["verifiedAt"] ?? string.Empty),
                Professions = (string)temp["professions"],
                Bio = (string)temp["bio"],
            };

            return View(user);
        }

        public async Task<ActionResult> AdminEdit(int id)

        {
            if (HttpContext.Session.GetString("r") == null || HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_userApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var user = JsonSerializer.Deserialize<Account>(strData, options);

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminEdit(int id, Account user)
        {
            if (HttpContext.Session.GetString("r") == null || HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var response = await _client.GetAsync($"{_userApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var userEdit = JsonSerializer.Deserialize<Account>(strData, options);

            if (userEdit == null) return View(user);

            userEdit.Professions = user.Professions;
            userEdit.FullName = user.FullName;
            userEdit.Role = user.Role;

            var userToEdit = JsonSerializer.Serialize(userEdit);
            var content = new StringContent(userToEdit, Encoding.UTF8, "application/json");
            response = await _client.PutAsync(_userApiUrl + "/" + id, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Can not Update";
                return View(user);
            }

            TempData["message"] = "Update Successfully";
            return RedirectToAction("AdminDetails", "User", new { Id = user.AccountId });
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") == null) return RedirectToAction("Index", "Home");

            if (!IsCurrentUser(id)) return NotFound();

            var token = HttpContext.Session.GetString("t");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Home");

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_userApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            var user = new Account()
            {
                AccountId = id,
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                FullName = (string)temp["fullName"],
                AvatarUrl = (string)temp["avatarUrl"],
                VerifiedAt = DateTime.Parse((string)temp["verifiedAt"] ?? string.Empty),
                Professions = (string)temp["professions"],
                Bio = (string)temp["bio"],
            };

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Account user)
        {
            if (HttpContext.Session.GetString("r") != "User")
                return RedirectToAction("Index", "Home");

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;

            var postedFile = Request.Form.Files.FirstOrDefault();
            if (postedFile != null)
            {
                var form = new MultipartFormDataContent();
                var fileStreamContent = new StreamContent(postedFile.OpenReadStream());
                fileStreamContent.Headers.ContentType =
                    new MediaTypeHeaderValue(postedFile.ContentType);
                form.Add(fileStreamContent, "postedFile", postedFile.FileName);
                response = await _client.PostAsync($@"{_userApiUrl}/changeAvatar/{id}", form);

                if (!response.IsSuccessStatusCode) return View(user);

                var src = await response.Content.ReadAsStringAsync();
                src = src.Replace("\"", "");
                src = _configuration["BaseUrl"] + "/" + src;
                HttpContext.Session.SetString("a", src);
            }

            response = await _client.GetAsync($"{_userApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var userEdit = JsonSerializer.Deserialize<Account>(strData, options);

            userEdit.Username = user.Username;
            userEdit.FullName = user.FullName;
            userEdit.Bio = user.Bio;
            userEdit.PhoneNumber = user.PhoneNumber;

            var userToEdit = JsonSerializer.Serialize(userEdit);
            var content = new StringContent(userToEdit, Encoding.UTF8, "application/json");
            response = await _client.PutAsync(_userApiUrl + "/" + id, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Can not Update";
                return View(user);
            }

            TempData["message"] = "Update Successfully";
            return RedirectToAction("Details", "User", new { Id = id });
        }

        private bool IsCurrentUser(int id)
        {
            string currentUserId = HttpContext.Session.GetString("i");
            if (currentUserId != id.ToString()) return false;
            return true;
        }

        public async Task<IActionResult> AdminIndex(string searchString)

        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string query = null;
            if (searchString != null)
                query = "/Search" + "?search=" + searchString;


            HttpResponseMessage response;
            if (query == null)
            {
                response = await _client.GetAsync(_userApiUrl);
            }
            else
            {
                response = await _client.GetAsync(_userApiUrl + query);
            }


            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);
            return View(listUsers);
        }

        public IActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminCreate(Account account)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formUser = new UserDto()
            {
                Email = account.Email,
                FullName = account.FullName,
                Username = account.Username,
                PhoneNumber = account.PhoneNumber,
                Professions = account.Professions,
                Role = account.Role,
                Password = account.Password,
                ConfirmPassword = account.Password
            };
            var newUser = JsonSerializer.Serialize(formUser);
            var content = new StringContent(newUser, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_userApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Can not Create";
                return View(account);
            }

            TempData["message"] = "Create Successfully";
            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public async Task<IActionResult> ManageRole()
        {
            var token = HttpContext.Session.GetString("t");
            if (HttpContext.Session.GetString("r") != "Admin" || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get users
            HttpResponseMessage response = await _client.GetAsync(_userApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);

            
            return View(listUsers);
        }

        [HttpGet]
        public async Task<IActionResult> ManageBlocked()
        {
            var token = HttpContext.Session.GetString("t");
            if (HttpContext.Session.GetString("r") != "Admin" || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get users
            HttpResponseMessage response = await _client.GetAsync(_userApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);

            
            return View(listUsers);
        }
    }
}