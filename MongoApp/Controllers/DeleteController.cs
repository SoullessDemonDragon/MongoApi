using Microsoft.AspNetCore.Mvc;
using MongoApp.DTO.DeleteDto;
using MongoApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MongoApp.Controllers
{
    public class DeleteController : Controller
    {
        public readonly HttpClient _httpClient;
        public DeleteController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteDto user, bool isAdminDelete = false)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    Content = content,
                    RequestUri = new Uri("api/UserData/DeactivateUser", UriKind.Relative)
                };

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "User Deleted Successfully";
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();

                    if (isAdminDelete)
                    {
                        return RedirectToAction("GetAllUser", "GetAllUser");
                    }
                    else
                    {
                        return RedirectToAction("Logout", "Logout");
                    }
                }
                else
                {
                    return View("Error", response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteUser1(UserData user)
        {
            try
            {
                var x = new DeleteDto()
                {
                    Id = user.Id,
                    Password = user.Password
                };
                return View("DeleteForSure", x);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AdminDeleteUser1(UserData user)
        {
            try
            {
                var x = new DeleteDto()
                {
                    Id = user.Id,
                    Password = user.Password
                };
                return View("AdminDeleteForSure", x);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
