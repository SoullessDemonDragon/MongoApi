using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoApp.DTO;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MongoApp.Controllers
{
    public class CreateController : Controller
    {
        public readonly HttpClient _httpClient;
        public CreateController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateUser(CreateDto user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/UserData/CreateUser", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "User Created successfully!";
                    return RedirectToAction("GetAllUser", "GetAllUser");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
                    return RedirectToAction(nameof(CreateUser), user);
                }
                else
                {
                    TempData["ToastMessage"] = $"Error: {response.ReasonPhrase}";
                    return RedirectToAction(nameof(CreateUser), user);
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(CreateUser), user);
            }
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly", AuthenticationSchemes = "MyAuthScheme", Roles = "Admin")]
        public IActionResult CreateUser()
        {
            try
            {
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("Create", new CreateDto());
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }
    }
}
