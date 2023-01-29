using LiteraturePlatformClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using Text = LiteraturePlatformClient.Models.Text;

namespace LiteraturePlatformClient.Controllers
{
    public class HomeController : Controller
    {
        IHttpClientFactory clientFactory;
        List<Genre> genres;
        public HomeController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Compositions = await GetAll();
            ViewBag.Top1 = await GetTop1Raiting();
            ViewBag.Latest = await GetLatest();
            ViewBag.All = await GetAll();

            if (User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login") != null)
            {
                ViewBag.Login = User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login").Value;               
            }

            return View();
        }

        public async Task<IList<Composition>> GetAll()
        {
            IList<Composition> Compositions = null;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Compositions = JsonConvert.DeserializeObject<IEnumerable<Composition>>(jsonString).ToList<Composition>();
            }
            return Compositions;
        }
        public async Task<Composition> GetTop1Raiting()
        {
            Composition Compositions = null;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetTop1Raiting");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Compositions = JsonConvert.DeserializeObject<Composition>(jsonString);
            }
            return Compositions;
        }
        public async Task<IList<Composition>> GetLatest()
        {
            IList<Composition> Compositions = null;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetTop2Latest");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Compositions = JsonConvert.DeserializeObject<IEnumerable<Composition>>(jsonString).ToList<Composition>();
            }
            return Compositions;
        }

        public async Task<List<Genre>> GetGenres()
        {
            List<Genre> Genres = null;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetGenres");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Genres = JsonConvert.DeserializeObject<IEnumerable<Genre>>(jsonString).ToList<Genre>();
            }
            return Genres;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Login = User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login").Value;
            ViewBag.Genres = await GetGenres();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Title, Description")] Composition composition, 
            [FromForm] int Genre, List<IFormFile> files)
        {
            if (files.Count == 2)
            {              
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.ToString() == "Id").Value);

                byte[] imageData = null;
                string text = null;
                using (var binaryReader = new BinaryReader(files[0].OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)files[0].Length);
                }
                

                using (var streamReader = new StreamReader(files[1].OpenReadStream()))
                {
                    text = streamReader.ReadToEnd();
                }

                SendModel model = new SendModel
                {
                    title = composition.Title,
                    descr = composition.Description,
                    userId = userId,
                    genreId = Genre,
                    imageData = imageData,
                    text = text
                };


                var client = clientFactory.CreateClient();

                var json = new StringContent(
                   System.Text.Json.JsonSerializer.Serialize(model),
                   Encoding.UTF8,
                   Application.Json);

                ViewBag.Genres = await GetGenres();//

                using (var response = await client.PostAsync("https://localhost:7285/Platform/CreateComposition", json))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return Redirect("Index");
                    }
                }
            }
            else
            {
                return BadRequest();
            }



            return View();
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login") != null)
            {
                ViewBag.Login = User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login").Value;
            }
            var comp = await GetComposition(id);
            return View(comp);
        }
        public async Task<Composition> GetComposition(int id)
        {
            Composition Compositions = null;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetComposition/"+ id);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Compositions = JsonConvert.DeserializeObject<Composition>(jsonString);
            }
            return Compositions;
        }

        [Authorize]
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment([FromBody]int compositionId, [FromBody]string content)
        {
            Text text = new Text()
            {
                TextId = compositionId,
                Content = content,
            };

            var client = clientFactory.CreateClient();
            var json = new StringContent(
                   System.Text.Json.JsonSerializer.Serialize(text),
                   Encoding.UTF8,
                   Application.Json);

            using (var request = await client.PostAsync("https://localhost:7285/Platform/AddComment", json))
            {
                if (request.IsSuccessStatusCode)
                {
                    return Redirect("Details");
                }
            }
            return BadRequest();
        }
    }
}