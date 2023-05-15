using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetectionHistoryController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IDetectionHistoryRepository _detectionHistoryRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        public DetectionHistoryController(IDetectionHistoryRepository detectionHistoryRepository, IMapper mapper,
            AppDbContext context, IHostingEnvironment environment, IConfiguration configuration)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);


            _detectionHistoryRepository = detectionHistoryRepository;
            _mapper = mapper;
            _context = context;
            _environment = environment;

            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var postedFile = Request.Form.Files.FirstOrDefault();
            var accountId = Request.Form["uid"].FirstOrDefault();

            if (postedFile == null)
                return BadRequest("File is null or empty");

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            string path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = Path.GetFileName(postedFile.FileName);
            var uniqueFilePath = Path.Combine(path, fileName);
            var uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            // Check if file name exist, use Windows style rename
            if (System.IO.File.Exists(uniqueFilePath))
            {
                var count = 1;

                var extension = Path.GetExtension(uniqueFilePath);
                var newFullPath = uniqueFilePath;

                while (System.IO.File.Exists(Path.Combine(path, newFullPath)))
                {
                    var tempFileName = $"{uniqueFileName} ({count++})";
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }

                uniqueFilePath = newFullPath;
                uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            }

            await using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);
            stream.Close();

            // Save image location to database
            _context.Images.Add(new Image { AltText = uniqueFileName, Url = uniqueFilePath });
            await _context.SaveChangesAsync();

            if (accountId != null)
            {
                var history = new DetectionHistory
                {
                    AccountId = int.Parse(accountId),
                    Date = DateTime.Now,
                    ImageName = uniqueFileName,
                    ImageUrl = uniqueFilePath,
                };
                await SaveHistory(history);

                var detectedDisease = await RetrieveAccuraciesFromApi(history, uniqueFilePath);
                history.DetectedDisease = detectedDisease;
                await _context.SaveChangesAsync();
            }

            var responseMessage = $"{fileName} uploaded successfully";
            return Ok(responseMessage);
        }

        private async Task<Disease> RetrieveAccuraciesFromApi(DetectionHistory history, string filepath)
        {
            var apiBaseUrl = _configuration["PythonApiUrl"] + "/predict";
            var detectingDiseases = _context.Diseases.Where(
                d => d.Name == "Early Blight"
                     || d.Name == "Septoria"
                     || d.Name == "Yellow Curl" ||
                     d.Name == "Healthy Leaf").ToList();
            List<PredictionDto> data;

            using (var multipartFormContent = new MultipartFormDataContent())
            {
                //Load the file and set the file's Content-Type header
                FileStream temp = null;
                try
                {
                    temp = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (temp != null)
                {
                    var fileStreamContent = new StreamContent(temp);
                    var filename = Path.GetFileName(filepath);

                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                    //Add the file
                    multipartFormContent.Add(fileStreamContent, name: "file", fileName: filename);
                }

                //Send it
                var response = await _client.PostAsync(apiBaseUrl, multipartFormContent);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<List<PredictionDto>>(result);
            }

            var detectedDisease = new Disease();
            double maxAcc = 0;

            for (var i = 0; i < detectingDiseases.Count; i++)
            {
                var acc = new DetectionAccuracy
                {
                    Accuracy = data[i].Probability,
                    DiseaseId = detectingDiseases[i].DiseaseId,
                    DetectionHistoryId = history.DetectionHistoryId
                };

                if (acc.Accuracy > maxAcc)
                {
                    maxAcc = acc.Accuracy;
                    detectedDisease = acc.Disease;
                }

                _context.DetectionAccuracies.Add(acc);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return detectedDisease;
        }

        /// <summary>
        /// Save image history
        /// </summary>
        /// <param name="history">The image to save into history</param>
        /// <returns></returns>
        private async Task SaveHistory(DetectionHistory history)
        {
            if (history == null) return;
            _context.DetectionHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        [HttpGet("{accountId}")]
        public IActionResult GetDetectionHistories(int accountId)
        {
            var detectionHistories =
                _mapper.Map<List<DetectionHistory>>(_detectionHistoryRepository.GetDetectionHistories(accountId));


            var result = new List<ExtractDetectionHistoriesDto>();
            foreach (var detection in detectionHistories)
            {
                var accuracies = GetDetectionAccuracies(detection.DetectionHistoryId).Include(x => x.Disease);
                var highestAcc = accuracies.Max(x => x.Accuracy);
                var detectedDisease = accuracies.First(x => x.Accuracy == highestAcc);

                result.Add(new ExtractDetectionHistoriesDto
                {
                    DetectionHistoryId = detection.DetectionHistoryId,
                    ImageName = detection.ImageName,
                    DetectedDisease = detectedDisease.Disease.Name,
                    Accuracy = (float)highestAcc,
                    ImageUrl = detection.ImageUrl,
                    IsExpertConfirmed = detection.IsExpertConfirmed
                });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var detectionHistories =
                _mapper.Map<List<DetectionHistory>>(_detectionHistoryRepository.GetAll());

            var result = new List<ExtractDetectionHistoriesDto>();
            foreach (var detection in detectionHistories)
            {
                var accuracies = GetDetectionAccuracies(detection.DetectionHistoryId).Include(x => x.Disease);
                var highestAcc = accuracies.Max(x => x.Accuracy);
                var detectedDisease = accuracies.First(x => x.Accuracy == highestAcc);

                result.Add(new ExtractDetectionHistoriesDto
                {
                    DetectionHistoryId = detection.DetectionHistoryId,
                    ImageName = detection.ImageName,
                    DetectedDisease = detectedDisease.Disease.Name,
                    Accuracy = (float)highestAcc,
                    ImageUrl = detection.ImageUrl,
                    IsExpertConfirmed = detection.IsExpertConfirmed
                });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        // For testing
        [HttpGet("Details/{detectionHistoryId}")]
        public IActionResult GetDetectionHistory(int detectionHistoryId)
        {
            if (!_detectionHistoryRepository.Exist(detectionHistoryId))
                return NotFound($"Detection history '{detectionHistoryId}' is not exists!!");

            var accuracies = GetDetectionAccuracies(detectionHistoryId)
                .Where(x => x.DetectionHistoryId == detectionHistoryId)
                .OrderByDescending(x => x.Accuracy)
                .Include(x => x.Disease)
                .Include(x => x.Disease.Medicine)
                .Include(x => x.Disease.Treatment)
                .ToList();

            return Ok(accuracies);
        }

        private IQueryable<DetectionAccuracy> GetDetectionAccuracies(int historyId)
        {
            //TODO: Disease is null now
            var result =
                from acc in _context.DetectionAccuracies
                join disease in _context.Diseases
                    on acc.DiseaseId equals disease.DiseaseId
                orderby acc.Accuracy descending
                where acc.DetectionHistoryId == historyId
                select acc;

            return result;
        }
    }
}