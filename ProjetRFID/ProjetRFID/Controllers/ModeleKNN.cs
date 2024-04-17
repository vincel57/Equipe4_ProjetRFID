using Microsoft.AspNetCore.Mvc;
using ProjetRFID.Models;
using System.Diagnostics;

namespace ProjetRFID.Controllers
{
    public class ModeleKNNController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
