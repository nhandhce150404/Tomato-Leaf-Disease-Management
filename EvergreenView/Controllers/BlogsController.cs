using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.Extensions.Configuration;


namespace EvergreenView.Controllers
{
    public class BlogsController : Controller
    {
        private readonly string _blogApiUrl;
        private readonly string _thumbnailApiUrl;
        private readonly HttpClient _client;

        public BlogsController(IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);


            _blogApiUrl = configuration["BaseUrl"] + "/api/Blog";
            _thumbnailApiUrl = configuration["BaseUrl"] + "/api/Thumbnail";

        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync(_blogApiUrl);

            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Blog> listBlogs = JsonSerializer.Deserialize<List<Blog>>(strData, options);
            return View(listBlogs);
        }


        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var blog = await GetBlogById(id);
            if (blog == null)
                return NotFound();
            return View(blog);
        }

        public async Task<ActionResult> Read(int id)
        {
            var blog = await GetBlogById(id);
            if (blog == null)
                return NotFound();
            var view = await UpdateViewBlogById(id);
            if (view == true)
            {
                return View(blog);
            }

            return View(blog);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await _client.GetAsync(_thumbnailApiUrl);
            string strData1 = await response.Content.ReadAsStringAsync();
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
        public async Task<IActionResult> Create(Blog p)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }


            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            p.LastModifiedDate = DateTime.Now.AddHours(7);
            string data = JsonSerializer.Serialize(p);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_blogApiUrl, content).Result;
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

            HttpResponseMessage response = await _client.GetAsync(_blogApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var blog = JsonSerializer.Deserialize<Blog>(strData, options);
            var responseImage = await _client.GetAsync(_thumbnailApiUrl);
            var strData2 = await responseImage.Content.ReadAsStringAsync();

            var listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            return View(blog);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Blog blog)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            blog.LastModifiedDate = DateTime.Now.AddHours(7);
            var blogId = await GetBlogById(id);
            var data = JsonSerializer.Serialize(blog);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_blogApiUrl + "/" + id, content);


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
            return View(blogId);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var blog = await GetBlogById(id);
            if (blog == null)
                return NotFound();
            await SetViewData();
            return View(blog);
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

            var blog = await GetBlogById(id);

            HttpResponseMessage response = await _client.DeleteAsync(_blogApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }

            TempData["error"] = "Can not Delete";
            return View(blog);
        }


        private async Task<bool> UpdateViewBlogById(int blogid)
        {
            var blog = await GetBlogById(blogid);
            var data = JsonSerializer.Serialize(blog);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_blogApiUrl + "/UpdateViewBlog/" + blogid, content);
            if (!response.IsSuccessStatusCode)
                return false;


            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return true;
        }


        private async Task<Blog> GetBlogById(int id)
        {
            HttpResponseMessage response = await _client.GetAsync(_blogApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;


            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Blog>(strData, options);
        }


        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await _client.GetAsync(_blogApiUrl);
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
                response = await _client.GetAsync(_blogApiUrl);
            }
            else
            {
                response = await _client.GetAsync(_blogApiUrl + query);
            }


            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            List<Blog> listBlogs = JsonSerializer.Deserialize<List<Blog>>(strData, options);
            return View(listBlogs);
        }
    }
}