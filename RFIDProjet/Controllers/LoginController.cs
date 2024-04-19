using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRFID.Model;
using RFIDProjet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RFIDProjet.ControllersAPI;

namespace RFIDProjet.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            UserViewModel viewModel = new UserViewModel { Authentifie = false };
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    viewModel.User = API.Instance.GetUser(HttpContext.User.Identity.Name).Result;
            //}
            //else
            //{
            //    HttpContext.User.Identity.;
            //}
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UserE usere = API.Instance.GetUserE(viewModel.usere.loginE, viewModel.usere.passwordE).Result;
                if (usere != null)
                {
                    return Redirect("/Home/Index");
                }
                ModelState.AddModelError("usere.loginE", "Login et/ou mot de passe incorrect(s)");
            
                }
            return View(viewModel);
        }

        public ActionResult Deconnexion()
        {
            return Redirect("/");
        }
    }
}