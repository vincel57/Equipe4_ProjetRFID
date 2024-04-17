using Microsoft.AspNetCore.Mvc;
using ProjetRFID.Models;
using System.Diagnostics;

namespace ProjetRFID.Controllers
{
    public class ModeleRandomController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}