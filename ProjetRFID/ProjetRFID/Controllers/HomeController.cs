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
using Newtonsoft.Json.Linq;
using System.Text;




namespace ProjetRFID.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
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
                return RedirectToAction("Admin", "Home");
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
        public IActionResult Edit()
        {
            // Ajoutez ici le code nécessaire pour préparer la vue
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Traitement(string Checkbox_Analytique, string Checkbox_KNN, string Checkbox_RandomForest, string Checkbox_SVM,
    int n_neighbors, string weight, string metric, float p, string metric_params, string algorithm,
    int leaf_size, int n_estimators, string criterion, int min_samples_split, int min_samples_leaf,
    float min_weight_fraction_leaf, int max_leaf_nodes, float min_impurity_decrease, int n_jobs,
    int entier_detail, int max_depth, float C, string kernel, string gamma, float coef0, float tol,
    float cache_size, int max_iter, string decision_function_shape, IFormFile file, float test)
        {
            //méthode pour convertir en float
            float ExtractNumber(string resultA)
            {
                if (!string.IsNullOrEmpty(resultA))
                {

                    dynamic jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultA);
                    if (jsonResult != null && jsonResult.accuracy != null)
                    {
                        //  accuracy
                        return (float)jsonResult.accuracy;
                    }
                }
                return 0;
            }
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var userName = user?.UserName;

            HttpClient client = new HttpClient();

            // Analytique
            if (!string.IsNullOrEmpty(Checkbox_Analytique) && Checkbox_Analytique.Equals("Analytique"))
            {
                // Appel API
                var response = await client.PostAsync("http://localhost:5000/result", null);
                var resultA = await response.Content.ReadAsStringAsync();

                ViewBag.ResultAnalytique = resultA;

                var resultatJson = JObject.Parse(resultA);
                float precision = (float) resultatJson.GetValue("Analytique");
                // Enregistrement des données
                Analytique analytique = new Analytique()
                    {
                       precison = precision,
                    };
                    _context.Analytique.Add(analytique);
                    await _context.SaveChangesAsync();

                    Simulation simulation = new Simulation()
                    {
                        UserId = userId,
                        idA = analytique.id,
                        time = DateTime.Now,
                        UserName = userName,
                    };
                    _context.Simulation.Add(simulation); // Utilisez Add au lieu de Update pour ajouter une nouvelle entrée
                    await _context.SaveChangesAsync();
                }
              
            

            // KNN
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
                    test
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/knn", content);
                var resultk = await response.Content.ReadAsStringAsync();
                ViewBag.ResultKNN = resultk;
                var resultatJson = JObject.Parse(resultk);
                float precisionk = (float)resultatJson.GetValue("KNN");

                KNN knn = new KNN()
                {
                    n_neighbors = n_neighbors,
                    weight = weight,
                    metric = metric,
                    p = p,
                    metric_params = metric_params,
                    algorithm = algorithm,
                    leaf_size = leaf_size,
                    precision=precisionk,
                };
                _context.KNN.Add(knn);
                await _context.SaveChangesAsync();
                Simulation simulation = new Simulation()
                {
                    UserId = userId,
                    idk = knn.id,
                    time = DateTime.Now,
                    UserName = userName,
                };
                _context.Add(simulation);
                await _context.SaveChangesAsync();
            }

            // RandomForest
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
                    test
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/random", content);
                var resultr = await response.Content.ReadAsStringAsync();
                ViewBag.ResultRandomForest = resultr;
                var resultatJson = JObject.Parse(resultr);
                float precisionr = (float)resultatJson.GetValue("RandomForest");

                Random_Forest randomforest = new Random_Forest()
                {
                    n_estimators = n_estimators,
                    criterion = criterion,
                    min_samples_split = min_samples_split,
                    min_samples_leaf = min_samples_leaf,
                    min_weight_fraction_leaf = (int)min_weight_fraction_leaf,
                    max_leaf_nodes = max_leaf_nodes,
                    min_impurity_decrease = min_impurity_decrease,
                    n_jobs = n_jobs,
                    entier_detail = entier_detail,
                    max_depth = max_depth,
                    precision=precisionr,
                };
                _context.Random_Forest.Add(randomforest);
                await _context.SaveChangesAsync();
                Simulation simulation = new Simulation()
                {
                    UserId = userId,
                    idR = randomforest.id,
                    time = DateTime.Now,
                    UserName = userName,
                };
                _context.Add(simulation);
                await _context.SaveChangesAsync();
            }

            // SVM
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
                    test
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/svm", content);
                var results = await response.Content.ReadAsStringAsync();
                ViewBag.ResultSVM = results;
                var resultatJson = JObject.Parse(results);
                float precisions= (float)resultatJson.GetValue("SVM");



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
                    precision = precisions,
                };
                _context.SVM.Add(svm);
                await _context.SaveChangesAsync();
                Simulation simulation = new Simulation()
                {
                    UserId = userId,
                    idS = svm.id,
                    time = DateTime.Now,
                    UserName = userName,
                };
                _context.Add(simulation);
                await _context.SaveChangesAsync();
            }

            // Extraire les valeurs des ViewBag
            float? GetValueFromResult(string result)
            {
                if (string.IsNullOrEmpty(result)) return null;
                var json = JsonConvert.DeserializeObject<Dictionary<string, float>>(result);
                return json.Values.FirstOrDefault();
            }

            var ResultAnalytiqueValue = GetValueFromResult(ViewBag.ResultAnalytique);
            var ResultKNNValue = GetValueFromResult(ViewBag.ResultKNN);
            var ResultRandomForestValue = GetValueFromResult(ViewBag.ResultRandomForest);
            var ResultSVMValue = GetValueFromResult(ViewBag.ResultSVM);

            // return DownloadPdf(ResultAnalytiqueValue, ResultKNNValue, ResultRandomForestValue, ResultSVMValue);
            // Créer un objet dynamique contenant uniquement les valeurs non nulles
            var req = new
            {
                Checkbox_SVM,
                Checkbox_Analytique,
                Checkbox_KNN,
                Checkbox_RandomForest,
                ResultAnalytique = ResultAnalytiqueValue,
                ResultKNN = ResultKNNValue,
                ResultRandomForest = ResultRandomForestValue,
                ResultSVM = ResultSVMValue
            };

            var con = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync("http://localhost:5000/compare", con);
            var res = await resp.Content.ReadAsStringAsync();

            ViewBag.Result = res;
            // Redirection vers la vue Index avec tous les résultats
            return View("Index");

            // Redirection vers la vue Index avec tous les résultats
        }

        [HttpPost]
        public IActionResult DownloadPdf(string resultAnalytique, string resultKNN, string resultRandomForest, string resultSVM)
        {
            // Créer un document PDF
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            // Mémoire tampon pour stocker le PDF
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                // Créer un écrivain PDF
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                // Ouvrir le document
                pdfDoc.Open();

                // Ajouter les détails du formulaire
                pdfDoc.Add(new Paragraph("Détails du formulaire :"));
                foreach (string key in Request.Form.Keys)
                {
                    string value = Request.Form[key];
                    pdfDoc.Add(new Paragraph($"{key}: {value}"));
                }

                // Ajouter les résultats des ViewBag
                pdfDoc.Add(new Paragraph("\nComparaison entre les méthodes choisies :"));

                if (!string.IsNullOrEmpty(resultAnalytique))
                {
                    pdfDoc.Add(new Paragraph("Résultat Analytique:"));
                    pdfDoc.Add(new Paragraph(resultAnalytique));
                }

                if (!string.IsNullOrEmpty(resultKNN))
                {
                    pdfDoc.Add(new Paragraph("Résultat KNN:"));
                    pdfDoc.Add(new Paragraph(resultKNN));
                }

                if (!string.IsNullOrEmpty(resultRandomForest))
                {
                    pdfDoc.Add(new Paragraph("Résultat Random Forest:"));
                    pdfDoc.Add(new Paragraph(resultRandomForest));
                }

                if (!string.IsNullOrEmpty(resultSVM))
                {
                    pdfDoc.Add(new Paragraph("Résultat SVM:"));
                    pdfDoc.Add(new Paragraph(resultSVM));
                }

                // Ajouter l'image
                string imagePath = "'C:/Users/33760/Downloads/Equipe4_ProjetRFID/ProjetRFID/ProjetRFID/wwwroot/img/comparaison_.png";
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
                pdfDoc.Add(image);

                // Fermer le document
                pdfDoc.Close();

                // Convertir le document en tableau d'octets
                byte[] pdfBytes = memoryStream.ToArray();

                // Retourner le PDF en tant que fichier
                return File(pdfBytes, "application/pdf", "FormulaireEtResultats.pdf");
            }
            finally
            {
                // Nettoyer les ressources
                memoryStream.Dispose();
                pdfDoc.Dispose();
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        



