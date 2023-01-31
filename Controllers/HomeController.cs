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
            await initPage();
            ViewBag.All = await GetAll();

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
            Composition comp = await GetComposition(id);
            if (User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login") != null)
            {
                ViewBag.Login = User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login").Value;
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.ToString() == "Id").Value);
                ViewBag.CurrRate = await CurrRate(comp.CompositionId, userId);
            }

            return View(comp);
        }
        public async Task<Composition> GetComposition(int id)
        {
            Composition Compositions = null;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetComposition/" + id);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Compositions = JsonConvert.DeserializeObject<Composition>(jsonString);
            }
            return Compositions;
        }

        public async Task<int> CurrRate(int composId, int userId)
        {
            int res = 0;
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7285/Platform/CurrRate/{userId}/{composId}");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<int>(jsonString);
            }
            return res;
        }

        [Authorize]
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment([FromForm] int compositionId, [FromForm] string content)
        {
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.ToString() == "Id").Value);
            Comment text = new Comment()
            {
                CommentId = compositionId,
                UserId = userid,
                Text = content,

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
                    return Redirect("Details/" + compositionId);
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("FindByGenre/{id}")]
        public async Task<IActionResult> FindByGenre(int id)
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/FindByGenre/" + id);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await initPage();
                string jsonString = await response.Content.ReadAsStringAsync();
                ViewBag.All = JsonConvert.DeserializeObject<IEnumerable<Composition>>(jsonString).ToList<Composition>();

                return View("Index");
            }
            return BadRequest();
        }


        [HttpGet]
        [Route("GetTop50Rating")]
        public async Task<IActionResult> GetTop50Rating()
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetTop50Rating");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await initPage();
                string jsonString = await response.Content.ReadAsStringAsync();
                ViewBag.All = JsonConvert.DeserializeObject<IEnumerable<Composition>>(jsonString).ToList<Composition>();
                return View("Index");
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("GetTop50Comments")]
        public async Task<IActionResult> GetTop50Comments()
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7285/Platform/GetTop50Comments");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await initPage();
                string jsonString = await response.Content.ReadAsStringAsync();
                ViewBag.All = JsonConvert.DeserializeObject<IEnumerable<Composition>>(jsonString).ToList<Composition>();
                return View("Index");
            }
            return BadRequest();
        }
        async Task<bool> initPage()
        {
            ViewBag.Compositions = await GetAll();
            ViewBag.Top1 = await GetTop1Raiting();
            ViewBag.Latest = await GetLatest();
            ViewBag.Genres = await GetGenres();

            if (User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login") != null)
            {
                ViewBag.Login = User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login").Value;
            }
            return true;
        }

        [Authorize]
        [HttpPost]
        [Route("Rate")]
        public async Task<IActionResult> Rate([FromForm] int composId, [FromForm] int rating)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.ToString() == "Id").Value);

            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7285/Platform/Rate/{composId}/{userId}/{rating}");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return Redirect($"Details/{composId}");
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        [Route("Account")]
        public async Task<IActionResult> Account()
        {
            if (User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login") != null)
            {
                ViewBag.Login = User.Claims.FirstOrDefault(x => x.Type.ToString() == "Login").Value;
            }

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.ToString() == "Id").Value);

            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7285/Platform/AccountData/{userId}");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                User res = JsonConvert.DeserializeObject<User>(jsonString);

                return View(res);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Account")]
        public async Task<IActionResult> Account(User user)
        {
            var client = clientFactory.CreateClient();

            var json = new StringContent(
               System.Text.Json.JsonSerializer.Serialize(user),
               Encoding.UTF8,
               Application.Json);

            using (var response = await client.PostAsync("https://localhost:7285/Platform/ChangeData", json))
            {
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = await response.Content.ReadAsStringAsync();
                    return View();
                }
            }

            return BadRequest();
        }
        [HttpGet]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7285/Platform/DeleteUser/{id}");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Logout", "Account");
            }

            return BadRequest();
        }
    }
}