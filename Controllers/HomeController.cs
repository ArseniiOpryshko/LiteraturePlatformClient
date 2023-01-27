using LiteraturePlatformClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

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

        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await GetGenres();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title, Description")] Composition composition, 
            [FromForm] int Genre, List<IFormFile> files)
        {
            if (files.Count == 2)
            {
                byte[] imageData = null;
                string text = null;
                var imgfile = files[0];

                using (var binaryReader = new BinaryReader(imgfile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)imgfile.Length);
                }

                using (var streamReader = new StreamReader(files[1].OpenReadStream()))
                {
                    text = streamReader.ReadToEnd();
                }               
                composition.Image = imageData;


            }
            else
            {
                return BadRequest();
            }



            return View();
        }
    }
}