using Microsoft.AspNetCore.Mvc;
using RFIDProjet.Models;
using System.Diagnostics;

namespace RFIDProjet.Controllers
{
    public class ModeleSVMController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}