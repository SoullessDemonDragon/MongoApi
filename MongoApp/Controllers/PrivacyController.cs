using Microsoft.AspNetCore.Mvc;

namespace MongoApp.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
