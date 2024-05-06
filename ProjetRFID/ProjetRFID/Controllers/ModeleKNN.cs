/**using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetRFID.Models;
//using ProjetRFID.ControllersAPI;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace ProjetRFID.Controllers
{
    public class ModeleKNNController : Controller
    {

       // public IActionResult Index()
        {
          //  IEnumerable<KNN> knn_model = API.Instance.GetKNNsAsync().Result;
           // return View(knn_model);
        }

        // Post : KNN/create

        [HttpPost]
        [ValidateAntiForgeryToken]
       // public IActionResult Create([Bind("id,n_neighbors,weight,metric,p,metric_params,algorithm,leaf_size,date_simulation")] KNN knn_model)
        {
            if (ModelState.IsValid)
            {
               // var URI = API.Instance.AjoutEcurieAsync(knn_model);
                return RedirectToAction(nameof(Index));
            }
            return View(knn_model);
        }

    }
}
**/