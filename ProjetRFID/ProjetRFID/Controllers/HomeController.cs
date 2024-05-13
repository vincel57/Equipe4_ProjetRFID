using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetRFID.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
//using ProjetRFID.DAL;

namespace ProjetRFID.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Simple"))
            {
                return RedirectToAction("Index1", "Home");
            }
            else if (User.IsInRole("Expert"))
            {
                return RedirectToAction("Indexx", "Home");
            }
            else
                return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        [Authorize(Roles = "Expert")]
        public IActionResult Indexx()
        {
            return View("Index");
        }
        [Authorize(Roles = "Simple")]
        public IActionResult Index1()
        {
            return View();
        }
        [Authorize(Roles = "Expert")]
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    


}
