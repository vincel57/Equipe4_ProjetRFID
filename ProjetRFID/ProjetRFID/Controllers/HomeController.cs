using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetRFID.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using ProjetRFID.Controllers;
using Microsoft.EntityFrameworkCore;
using ProjetRFID.Data;
using System.IO;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol;
using System.Security.Claims;
using System.Drawing;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.Text.Json;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Document = iTextSharp.text.Document;
using System.Drawing.Printing;




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
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Historiques");
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
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [Authorize(Roles = "Expert")]
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            var histories = await _context.Historique.ToListAsync();
            return View(histories);
        }

        [HttpPost]
        public async Task<ActionResult> Result(
   string Checkbox_Analytique, string Checkbox_KNN, string Checkbox_RandomForest, string Checkbox_SVM,
   int n_neighbors, string weight, string metric, float p, string metric_params, string algorithm,
   int leaf_size, int n_estimators, string criterion, int min_samples_split, int min_samples_leaf,
   float min_weight_fraction_leaf, int max_leaf_nodes, float min_impurity_decrease, int n_jobs,
   int entier_detail, int max_depth, float C, string kernel, string gamma, float coef0, float tol,
   float cache_size, int max_iter, string decision_function_shape, IFormFile file, float testK,
   float EntraineK, float testR, float EntraineR, float testS, float EntraineS)
        {
            // Gestion du fichier (si nécessaire)
            string filePath = null;
            if (file != null)
            {
                var directory = "C:\\YourDirectory\\Uploads";
                var fileName = Path.GetFileName(file.FileName);
                filePath = Path.Combine(directory, fileName);

                // Créer le répertoire s'il n'existe pas
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Enregistrer le fichier dans le répertoire spécifié
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            HttpClient client = new HttpClient();

            if (!string.IsNullOrEmpty(Checkbox_Analytique) && Checkbox_Analytique.Equals("Analytique"))
            {
                var response = await client.PostAsync("http://localhost:5000/result", null);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.ResultAnalytique = result;
            }

            if (!string.IsNullOrEmpty(Checkbox_KNN) && Checkbox_KNN.Equals("KNN"))
            {
                var requestData = new
                {
                    n_neighbors,
                    weight,
                    metric,
                    p,
                    metric_params,
                    algorithm,
                    leaf_size,
                    testK
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/knn", content);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.ResultKNN = result;
            }

            if (!string.IsNullOrEmpty(Checkbox_RandomForest) && Checkbox_RandomForest.Equals("RandomForest"))
            {
                var requestData = new
                {
                    n_estimators,
                    criterion,
                    min_samples_split,
                    min_samples_leaf,
                    min_weight_fraction_leaf,
                    max_leaf_nodes,
                    min_impurity_decrease,
                    n_jobs,
                    max_depth,
                    testR
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/random", content);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.ResultRandomForest = result;
            }

            if (!string.IsNullOrEmpty(Checkbox_SVM) && Checkbox_SVM.Equals("SVM"))
            {
                var requestData = new
                {
                    C,
                    kernel,
                    gamma,
                    coef0,
                    tol,
                    cache_size,
                    max_iter,
                    decision_function_shape,
                    testS
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/svm", content);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.ResultSVM = result;
            }

            // Redirection vers la vue Index avec tous les résultats
            return View("Index");
        }



        [HttpPost]
        public async Task<ActionResult> Traitement(string checkbox_Analytique, string checkbox_KNN, string checkbox_RandomForest, string checkbox_SVM, int n_neighbors, string weight, string metric, float p,
    string metric_params, string algorithm, int leaf_size, int n_estimators, string criterion,
    int min_samples_split, int min_samples_leaf, float min_weight_fraction_leaf, int max_leaf_nodes,
    float min_impurity_decrease, int n_jobs, int entier_detail, int max_depth, float C, string kernel, string gamma,
    float coef0, float tol, float cache_size, int max_iter, string decision_function_shape)
        {
            if (checkbox_Analytique == "Analytique")
            {
                Analytique analytique = new Analytique();

                _context.Analytique.Add(analytique);
                await _context.SaveChangesAsync();
                Simulation simulation = new Simulation()
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Pour récupérer l'Id de la personne qui est connectée
                    idA = analytique.id,
                    time = DateTime.Now,
                };
                _context.Update(simulation);
                await _context.SaveChangesAsync();
            }

            if (checkbox_KNN == "KNN")
            {
                KNN knn = new KNN()
                {
                    n_neighbors = n_neighbors,
                    weight = weight,
                    metric = metric,
                    p = p,
                    metric_params = metric_params,
                    algorithm = algorithm,
                    leaf_size = leaf_size,
                };
                _context.KNN.Add(knn);
                await _context.SaveChangesAsync();
                Simulation simulation = new Simulation()
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Pour récupérer l'Id de la personne qui est connectée
                    idk = knn.id,
                    time = DateTime.Now,
                };

                _context.Add(simulation);
                await _context.SaveChangesAsync();
            }

            if (checkbox_RandomForest == "RandomForest")
            {
                Random_Forest randomforest = new Random_Forest()
                {
                    n_estimators = n_estimators,
                    criterion = criterion,
                    min_samples_split = min_samples_split,
                    min_samples_leaf = min_samples_leaf,
                    min_weight_fraction_leaf = (int) min_weight_fraction_leaf,
                    max_leaf_nodes = max_leaf_nodes,
                    min_impurity_decrease = min_impurity_decrease,
                    n_jobs = n_jobs,
                    entier_detail = entier_detail,
                    max_depth = max_depth,
                };
                _context.Random_Forest.Add(randomforest);
                await _context.SaveChangesAsync();
                Simulation simulation = new Simulation()
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Pour récupérer l'Id de la personne qui est connectée
                    idR = randomforest.id,
                    time = DateTime.Now,
                };

                _context.Add(simulation);
                await _context.SaveChangesAsync();
            }

            if (checkbox_SVM == "SVM")
            {
                using (var client = new HttpClient())
                {
                    SVM svm = new SVM()
                    {
                        C = C,
                        kernel = kernel,
                        gamma = gamma,
                        coef0 = coef0,
                        tol = tol,
                        cache_size = cache_size,
                        max_iter = max_iter,
                        decision_function_shape = decision_function_shape,
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(svm), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/svm", content);
                    var result_svm = await response.Content.ReadAsStringAsync();

                    ViewBag.Result = result_svm;
                    if (float.TryParse(result_svm, out float precisions))
                    {
                        return Json(new { precision = precisions });
                    }
                    svm.precision = precisions;

                    _context.SVM.Add(svm);
                    await _context.SaveChangesAsync();
                    Simulation simulation = new Simulation()
                    {
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Pour récupérer l'Id de la personne qui est connectée
                        idS = svm.id,
                        time = DateTime.Now,
                    };

                    _context.Add(simulation);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }

            // Ajouter un retour par défaut pour les chemins non traités
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult DownloadPdf()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                // Ajouter les détails du formulaire
                pdfDoc.Add(new Paragraph("Détails du formulaire :"));
                foreach (string key in Request.Form.Keys)
                {
                    string value = Request.Form[key];
                    pdfDoc.Add(new Paragraph($"{key}: {value}"));
                }

                // Ajouter les résultats des ViewBag
                pdfDoc.Add(new Paragraph("\nRésultats :"));

                if (ViewBag.ResultAnalytique != null)
                {
                    pdfDoc.Add(new Paragraph("Resultat Analytique:"));
                    pdfDoc.Add(new Paragraph(ViewBag.ResultAnalytique.ToString()));
                }

                if (ViewBag.ResultKNN != null)
                {
                    pdfDoc.Add(new Paragraph("Resultat KNN:"));
                    pdfDoc.Add(new Paragraph(ViewBag.ResultKNN.ToString()));
                }

                if (ViewBag.ResultRandomForest != null)
                {
                    pdfDoc.Add(new Paragraph("Resultat Random Forest:"));
                    pdfDoc.Add(new Paragraph(ViewBag.ResultRandomForest.ToString()));
                }

                if (ViewBag.ResultSVM != null)
                {
                    pdfDoc.Add(new Paragraph("Resultat SVM:"));
                    pdfDoc.Add(new Paragraph(ViewBag.ResultSVM.ToString()));
                }

                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();

                return File(bytes, "application/pdf", "FormulaireEtResultats.pdf");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        



