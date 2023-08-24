using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoApp.Models;
using Newtonsoft.Json;

namespace MongoApp.Controllers
{
    public class GetAllUserController : Controller
    {
        HttpClient _httpClient;
        public GetAllUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }
        [HttpGet]
        [Authorize(Policy = "AdminOnly", AuthenticationSchemes = "MyAuthScheme", Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/UserData/GetUser");
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    var users = JsonConvert.DeserializeObject<List<UserData>>(responseBody);
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    return View("GetAllUser", users);
                }
                else { return View("Error", response.ReasonPhrase); }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
