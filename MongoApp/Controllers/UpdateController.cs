using Microsoft.AspNetCore.Mvc;
using MongoApp.DTO.UpdateDto;
using MongoApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MongoApp.Controllers
{
    public class UpdateController : Controller
    {
        HttpClient _httpClient;
        public UpdateController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserUpdateDto user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/UserData/UpdateUser", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var updatedUser = JsonConvert.DeserializeObject<UserData>(responseBody);
                    TempData["ToastMessage"] = "User Updated Successfully.";
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    if (updatedUser.Role == "Admin")
                    {
                        return View("~/Views/Login/Admin/AdminProfile.cshtml", updatedUser);
                    }
                    return View("~/Views/Login/User/UserProfile.cshtml", updatedUser);
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
                    return RedirectToAction("UpdateUser", user);
                }
                else
                {
                    TempData["ToastMessage"] = "Some Unknown Error occured. Pls Try Again.";
                    return RedirectToAction("UpdateUser", user);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdateUser1(UserData user)
        {
            try
            {
                var x = new UserUpdateDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Age = user.Age,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Phonenumber = user.Phonenumber,
                    Role = user.Role,
                    Status = user.Status,
                };
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("UpdateUser", x);
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminUpdateUser(AdminUpdateDto user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/UserData/UpdateUser", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "User updated successfully!";
                    return RedirectToAction("GetAllUser", "GetAllUser");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    return View("AdminUpdateUser", user);
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
        public IActionResult AdminUpdateUser1(UserData user)
        {
            try
            {
                var x = new AdminUpdateDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Age = user.Age,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Phonenumber = user.Phonenumber,
                    Role = user.Role,
                    Status = user.Status,
                };
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("AdminUpdateUser", x);
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/UserData/UpdateUserPassword", content);
                if (response.IsSuccessStatusCode)
                {
                    return View("PasswordUpdateResponse");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["UpdateMessage"] = "Wrong Current Password. Pls Try Again.";
                    return RedirectToAction("UpdatePassword1", dto.Id);
                }
                else return BadRequest(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdatePassword1(string Id)
        {
            var x = new UpdatePasswordDto { Id = Id };
            ViewBag.ToastMessage = TempData["UpdateMessage"]?.ToString();
            return View("UpdatePassword", x);
        }
    }
}
