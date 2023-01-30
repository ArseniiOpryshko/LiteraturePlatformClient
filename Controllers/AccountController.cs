using LiteraturePlatformClient.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Reflection;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace LiteraturePlatformClient.Controllers
{
    public class AccountController : Controller
    {
        IHttpClientFactory clientFactory;
        public AccountController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(User user)
        {
            var client = clientFactory.CreateClient();
            var registermodelJson = new StringContent(
               System.Text.Json.JsonSerializer.Serialize(user),
               Encoding.UTF8,
               Application.Json);

            using (var response = await client.PostAsync("https://localhost:7285/Account/Login", registermodelJson))
            {
                if (response.IsSuccessStatusCode)
                {
                    string encodedJwt = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("Token", encodedJwt);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    ViewBag.LoginError = jsonString;
                }
            }

            return View();
        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var client = clientFactory.CreateClient();

                var registermodelJson = new StringContent(
                   System.Text.Json.JsonSerializer.Serialize(model),
                   Encoding.UTF8,
                   Application.Json);

                using(var response = await client.PostAsync("https://localhost:7285/Account/Register", registermodelJson))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        ViewBag.EmailError = jsonString;
                    }
                }           
            }
            return View(model);
        }
    }
}
