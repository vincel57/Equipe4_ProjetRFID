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
    public class Random_ForestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Random_ForestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Random_Forest
        public async Task<IActionResult> Index()
        {
              return _context.Random_Forest != null ? 
                          View(await _context.Random_Forest.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Random_Forest'  is null.");
        }

        // GET: Random_Forest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Random_Forest == null)
            {
                return NotFound();
            }

            var random_Forest = await _context.Random_Forest
                .FirstOrDefaultAsync(m => m.id == id);
            if (random_Forest == null)
            {
                return NotFound();
            }

            return View(random_Forest);
        }

        // GET: Random_Forest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Random_Forest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,n_estimators,criterion,min_samples_split,min_samples_leaf,min_weight_fraction_leaf,max_leaf_nodes,min_impurity_decrease,n_jobs,entier_detail,max_depth,precision")] Random_Forest random_Forest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(random_Forest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(random_Forest);
        }

        // GET: Random_Forest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Random_Forest == null)
            {
                return NotFound();
            }

            var random_Forest = await _context.Random_Forest.FindAsync(id);
            if (random_Forest == null)
            {
                return NotFound();
            }
            return View(random_Forest);
        }

        // POST: Random_Forest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,n_estimators,criterion,min_samples_split,min_samples_leaf,min_weight_fraction_leaf,max_leaf_nodes,min_impurity_decrease,n_jobs,entier_detail,max_depth,precision")] Random_Forest random_Forest)
        {
            if (id != random_Forest.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(random_Forest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Random_ForestExists(random_Forest.id))
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
            return View(random_Forest);
        }

        // GET: Random_Forest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Random_Forest == null)
            {
                return NotFound();
            }

            var random_Forest = await _context.Random_Forest
                .FirstOrDefaultAsync(m => m.id == id);
            if (random_Forest == null)
            {
                return NotFound();
            }

            return View(random_Forest);
        }

        // POST: Random_Forest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Random_Forest == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Random_Forest'  is null.");
            }
            var random_Forest = await _context.Random_Forest.FindAsync(id);
            if (random_Forest != null)
            {
                _context.Random_Forest.Remove(random_Forest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Random_ForestExists(int id)
        {
          return (_context.Random_Forest?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
