using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetRFID.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using ProjetRFID.Controllers;
using Microsoft.EntityFrameworkCore;
using ProjetRFID.Data;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol;
using System.Security.Claims;
using System.Drawing;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.Text.Json;


namespace ProjetRFID.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
                     else if (propertyName == "metric")
                     {
                         var metric = propertyValue.GetString();
                         // Faites quelque chose avec la valeur de la propriété 'metric'
                         System.Console.WriteLine($"Metrique : {metric}");
                     }
                     else if (propertyName == "metric_params")
                     {
                         var metric_params = propertyValue.GetString();
                         // Faites quelque chose avec la valeur de la propriété 'metric_params'
                         System.Console.WriteLine($"Paramètre_Métrique : {metric_params}");
                     }
                     else if (propertyName == "algorithm")
                     {
                         var algorithm = propertyValue.GetString();
                         // Faites quelque chose avec la valeur de la propriété 'algorithm'
                         System.Console.WriteLine($"Algorithme : {algorithm}");
                     }
                     else if (propertyName == "leaf_size")
                     {
                         var leaf_size = propertyValue.GetInt32();
                         // Faites quelque chose avec la valeur de la propriété 'leaf_size'
                         System.Console.WriteLine($"Taille_feuilles : {leaf_size}");
                     }
                     else if (propertyName == "leaf_size")
                     {
                         var leaf_size = propertyValue.GetInt32();
                         // Faites quelque chose avec la valeur de la propriété 'leaf_size'
                         System.Console.WriteLine($"Taille_feuilles : {leaf_size}");
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
        [HttpPost]
         public async Task<ActionResult> Traitement(string checkbox_Analytique, string checkbox_KNN, string checkbox_RandomForest, string checkbox_SVM, int n_neighbors, string weight, string metric,float p,
             string metric_params, string algorithm,int leaf_size,int n_estimators,string criterion,
             int min_samples_split,int min_samples_leaf,float min_weight_fraction_leaf,int max_leaf_nodes,
             float min_impurity_decrease,int n_jobs,int entier_detail,int max_depth,float C,string kernel,string gamma,
             float coef0, float tol, float cache_size,int max_iter, string decision_function_shape)
         {
             Console.WriteLine(gamma);
             if (checkbox_Analytique == "Analytique")
             {
                 Analytique analytique = new Analytique()
                 {
                 };
                 _context.Analytique.Add(analytique);
                 await _context.SaveChangesAsync();
                 
                 Simulation simulation = new Simulation
                 {
                     UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),//Pour récupérer l'Id de la personne qui est connectée
                     idA = analytique.id,
                     time = DateTime.Now,
                 };

                _context.Add(simulation);


                 await _context.SaveChangesAsync();

                
             }
            
             if (checkbox_KNN == "KNN")
             {
                 KNN knn = new KNN();
                 {
                      knn.n_neighbors= n_neighbors;
                     knn.weight= weight;
                     knn.metric = metric;
                     knn.p = p;
                     knn.metric_params = metric_params;
                     knn.algorithm = algorithm;
                     knn.leaf_size = leaf_size;
                 };
                     _context.KNN.Add(knn);
                     await _context.SaveChangesAsync();
                     Simulation simulation = new Simulation
                     {
                         UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),//Pour récupérer l'Id de la personne qui est connectée
                         idk = knn.id,
                         time = DateTime.Now,
                         
                     };

                     _context.Add(simulation);
                      await _context.SaveChangesAsync();
                 

             }
             if (checkbox_RandomForest == "RandomForest")
             {
                 Random_Forest randomforest = new Random_Forest();
                 {
                     randomforest.n_estimators= n_estimators;
                     randomforest.criterion = criterion;
                     randomforest.min_samples_split = min_samples_split;
                     randomforest.min_samples_leaf = min_samples_leaf;
                     min_weight_fraction_leaf = randomforest.min_weight_fraction_leaf;
                     randomforest.max_leaf_nodes = max_leaf_nodes;
                     randomforest.min_impurity_decrease = min_impurity_decrease;
                     randomforest.n_jobs = n_jobs;
                     randomforest.entier_detail = entier_detail;
                     randomforest.max_depth = max_depth;
                 };
                 _context.Random_Forest.Add(randomforest);
                     await _context.SaveChangesAsync();
                     Simulation simulation = new Simulation
                     {
                         UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),//Pour récupérer l'Id de la personne qui est connectée
                         idR = randomforest.id,
                         time = DateTime.Now,
                     };

                     _context.Add(simulation);
                     await _context.SaveChangesAsync();
               


             }
              if (checkbox_SVM == "SVM")
             {
                 SVM svm = new SVM();
                 {

                     svm.C = C;
                     svm.kernel = kernel;
                     svm.gamma = gamma;
                     svm.coef0 = coef0;
                     svm.tol = tol;
                     svm.cache_size = cache_size;
                     svm.max_iter = max_iter;
                     svm.decision_function_shape = decision_function_shape;
                 };
                 _context.SVM.Add(svm);
                     await _context.SaveChangesAsync();
                     Simulation simulation = new Simulation
                     {
                         UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),//Pour récupérer l'Id de la personne qui est connectée
                         idS = svm.id,
                         time = DateTime.Now,
                     };

                     _context.Add(simulation);
                     await _context.SaveChangesAsync();

                 


             }

             return View("Index");
         }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    


}
