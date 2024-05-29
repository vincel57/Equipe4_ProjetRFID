using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetRFID.Data;
using ProjetRFID.Models;

namespace ProjetRFID.Controllers
{
    public class SimulationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SimulationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Simulations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Simulation.Include(s => s.Analytique).Include(s => s.KNN).Include(s => s.Random_Forest).Include(s => s.SVM);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Simulations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Simulation == null)
            {
                return NotFound();
            }

            var simulation = await _context.Simulation
                .Include(s => s.Analytique)
                .Include(s => s.KNN)
                .Include(s => s.Random_Forest)
                .Include(s => s.SVM)
                .FirstOrDefaultAsync(m => m.id == id);
            if (simulation == null)
            {
                return NotFound();
            }
            if (simulation.idA.HasValue)
            {
                return RedirectToAction("Details", "Analytiques", new { id = simulation.idA });
            }
            if (simulation.idk.HasValue)
            {
                return RedirectToAction("Details", "KNNs", new { id = simulation.idk });
            }
             if (simulation.idR.HasValue)
            {
                return RedirectToAction("Details", "Random_Forest", new { id = simulation.idR });
            }
             if (simulation.idS.HasValue)
            {
                return RedirectToAction("Details", "SVMs", new { id = simulation.idS });
            }

            // Si aucune méthode spécifique n'est trouvée, affiche les détails de la simulation
            return View(simulation);
        }

            
        // GET: Simulations/Create
        public IActionResult Create()
        {
            ViewData["idA"] = new SelectList(_context.Analytique, "id", "id");
            ViewData["idk"] = new SelectList(_context.KNN, "id", "id");
            ViewData["idR"] = new SelectList(_context.Random_Forest, "id", "id");
            ViewData["idS"] = new SelectList(_context.SVM, "id", "id");
            return View();
        }

        // POST: Simulations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,time,idA,idS,idk,idR,UserId,UserName")] Simulation simulation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(simulation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idA"] = new SelectList(_context.Analytique, "id", "id", simulation.idA);
            ViewData["idk"] = new SelectList(_context.KNN, "id", "id", simulation.idk);
            ViewData["idR"] = new SelectList(_context.Random_Forest, "id", "id", simulation.idR);
            ViewData["idS"] = new SelectList(_context.SVM, "id", "id", simulation.idS);
            return View(simulation);
        }

        // GET: Simulations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Simulation == null)
            {
                return NotFound();
            }

            var simulation = await _context.Simulation.FindAsync(id);
            if (simulation == null)
            {
                return NotFound();
            }
            ViewData["idA"] = new SelectList(_context.Analytique, "id", "id", simulation.idA);
            ViewData["idk"] = new SelectList(_context.KNN, "id", "id", simulation.idk);
            ViewData["idR"] = new SelectList(_context.Random_Forest, "id", "id", simulation.idR);
            ViewData["idS"] = new SelectList(_context.SVM, "id", "id", simulation.idS);
            return View(simulation);
        }

        // POST: Simulations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,time,idA,idS,idk,idR,UserId,UserName")] Simulation simulation)
        {
            if (id != simulation.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(simulation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SimulationExists(simulation.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["idA"] = new SelectList(_context.Analytique, "id", "id", simulation.idA);
            ViewData["idk"] = new SelectList(_context.KNN, "id", "id", simulation.idk);
            ViewData["idR"] = new SelectList(_context.Random_Forest, "id", "id", simulation.idR);
            ViewData["idS"] = new SelectList(_context.SVM, "id", "id", simulation.idS);
            return View(simulation);
        }

        // GET: Simulations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Simulation == null)
            {
                return NotFound();
            }

            var simulation = await _context.Simulation
                .Include(s => s.Analytique)
                .Include(s => s.KNN)
                .Include(s => s.Random_Forest)
                .Include(s => s.SVM)
                .FirstOrDefaultAsync(m => m.id == id);
            if (simulation == null)
            {
                return NotFound();
            }

            return View(simulation);
        }

        // POST: Simulations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Simulation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Simulation'  is null.");
            }
            var simulation = await _context.Simulation.FindAsync(id);
            if (simulation != null)
            {
                _context.Simulation.Remove(simulation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index4(string filterName, DateTime? filterDate)
        {
            var simulations = from s in _context.Simulation
                              select s;

            if (!String.IsNullOrEmpty(filterName))
            {
                simulations = simulations.Where(s => s.UserName.Contains(filterName));
            }

            if (filterDate.HasValue)
            {
                simulations = simulations.Where(s => s.time.Date == filterDate.Value.Date);
            }

            ViewData["CurrentFilterName"] = filterName;
            ViewData["CurrentFilterDate"] = filterDate?.ToString("yyyy-MM-dd");

            return View(await simulations.ToListAsync());
        }

        private bool SimulationExists(int id)
        {
          return (_context.Simulation?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
