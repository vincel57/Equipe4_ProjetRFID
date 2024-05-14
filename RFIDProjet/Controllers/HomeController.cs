using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RFIDProjet.Models;
using System.Diagnostics;
using System.Text.Json;

namespace RFIDProjet.Controllers
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
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Analytical(IFormFile fichier)
        {
            var filePath = Path.Combine("Folder", fichier.FileName);

            // Enregistrer le fichier dans le répertoire spécifié
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fichier.CopyToAsync(stream);
            }

            // Créer un objet FormData avec le chemin du fichier au lieu du fichier lui-même
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(filePath), "filePath");

            // Envoyer la requête HTTP avec le chemin du fichier
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:5000/result", formData);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.Result = result;
            }

            return View("Index");
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
