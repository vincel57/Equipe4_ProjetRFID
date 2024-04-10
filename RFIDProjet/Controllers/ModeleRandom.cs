using Microsoft.AspNetCore.Mvc;
using RFIDProjet.Models;
using System.Diagnostics;

namespace RFIDProjet.Controllers
{
    public class ModeleRandomController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}