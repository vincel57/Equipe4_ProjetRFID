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
    public class KNNsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KNNsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KNNs
        public async Task<IActionResult> Index()
        {
              return _context.KNN != null ? 
                          View(await _context.KNN.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.KNN'  is null.");
        }

        // GET: KNNs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KNN == null)
            {
                return NotFound();
            }

            var kNN = await _context.KNN
                .FirstOrDefaultAsync(m => m.id == id);
            if (kNN == null)
            {
                return NotFound();
            }

            return View(kNN);
        }

        // GET: KNNs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KNNs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,n_neighbors,weight,metric,p,metric_params,algorithm,leaf_size,precision")] KNN kNN)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kNN);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kNN);
        }

        // GET: KNNs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KNN == null)
            {
                return NotFound();
            }

            var kNN = await _context.KNN.FindAsync(id);
            if (kNN == null)
            {
                return NotFound();
            }
            return View(kNN);
        }

        // POST: KNNs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,n_neighbors,weight,metric,p,metric_params,algorithm,leaf_size,precision")] KNN kNN)
        {
            if (id != kNN.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kNN);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KNNExists(kNN.id))
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
            return View(kNN);
        }

        // GET: KNNs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KNN == null)
            {
                return NotFound();
            }

            var kNN = await _context.KNN
                .FirstOrDefaultAsync(m => m.id == id);
            if (kNN == null)
            {
                return NotFound();
            }

            return View(kNN);
        }

        // POST: KNNs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KNN == null)
            {
                return Problem("Entity set 'ApplicationDbContext.KNN'  is null.");
            }
            var kNN = await _context.KNN.FindAsync(id);
            if (kNN != null)
            {
                _context.KNN.Remove(kNN);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KNNExists(int id)
        {
          return (_context.KNN?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
