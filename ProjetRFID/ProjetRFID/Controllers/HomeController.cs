using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetRFID.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
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

        [HttpPost]
        public IActionResult Traitement([FromBody] JsonElement formDataArray)
        {
            // Vérifiez si le JSON est un objet
            if (formDataArray.ValueKind == JsonValueKind.Object)
            {
                // Parcourir les propriétés de l'objet JSON
                foreach (var property in formDataArray.EnumerateObject())
                {
                    // Accédez aux valeurs des propriétés
                    var propertyName = property.Name;
                    var propertyValue = property.Value;

                    // Manipulez les valeurs selon vos besoins
                    if (propertyName == "method")
                    {
                        var method = propertyValue.GetString();
                        // Faites quelque chose avec la valeur de la propriété 'method'
                        System.Console.WriteLine($"Méthode : {method}");
                    }
                    else if (propertyName == "n_neighbors")
                    {
                        var n_neighbors = propertyValue.GetInt32();
                        // Faites quelque chose avec la valeur de la propriété 'n_neighbors'
                        System.Console.WriteLine($"Nombre de voisins : {n_neighbors}");
                    }
                    else if (propertyName == "weight")
                    {
                        var weight = propertyValue.GetString();
                        // Faites quelque chose avec la valeur de la propriété 'weight'
                        System.Console.WriteLine($"Poids : {weight}");
                    }
                    // Ajoutez d'autres conditions pour d'autres propriétés si nécessaire
                }
            }
            else
            {
                // Gérer le cas où le JSON n'est pas un objet
                System.Console.WriteLine("Le JSON n'est pas un objet");
            }

            // Vous pouvez également renvoyer les données pour confirmer qu'elles ont été reçues correctement
            return Ok(formDataArray);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    


}
