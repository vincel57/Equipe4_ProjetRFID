using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ProjetRFID.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace ProjetRFID.Controllers
{
    public class API : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
