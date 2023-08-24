using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoApp.DTO;
using MongoApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MongoApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/UserData/GetUserByUserName/username", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserData>(responseBody);
                    return await HandleLogin(user);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["ToastMessage"] = "Wrong Password. Please Try Again.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ToastMessage"] = "This Email is not in the database. Please Try Again.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception)
            {
                TempData["ToastMessage"] = "Server Not Responding. Please Try Again Later.";
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            try
            {
                if (Request.Cookies.TryGetValue("UserData", out var responseBody))
                {
                    var dto = JsonConvert.DeserializeObject<UserData>(responseBody);
                    return await HandleLogin(dto);
                }
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("Login", new LoginDto());
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        private async Task<IActionResult> HandleLogin(UserData user)
        {
            try
            {
                if (user.Role == "Admin")
                {
                    var identity = new ClaimsIdentity("MyAuthScheme");
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("MyAuthScheme", principal);
                    Response.Cookies.Append("UserData", JsonConvert.SerializeObject(user));
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    return View("Admin/AdminProfile", user);
                }
                Response.Cookies.Append("UserData", JsonConvert.SerializeObject(user));
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("User/UserProfile", user);
            }
            catch (Exception)
            {
                TempData["ToastMessage"] = "Server Not Responding. Please Try Again Later.";
                return RedirectToAction("Login");
            }
        }

    }
}
