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
    public class AnalytiquesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalytiquesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analytiques
        public async Task<IActionResult> Index()
        {
              return _context.Analytique != null ? 
                          View(await _context.Analytique.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Analytique'  is null.");
        }

        // GET: Analytiques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Analytique == null)
            {
                return NotFound();
            }

            var analytique = await _context.Analytique
                .FirstOrDefaultAsync(m => m.id == id);
            if (analytique == null)
            {
                return NotFound();
            }

            return View(analytique);
        }

        // GET: Analytiques/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Analytiques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,precison")] Analytique analytique)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analytique);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analytique);
        }

        // GET: Analytiques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Analytique == null)
            {
                return NotFound();
            }

            var analytique = await _context.Analytique.FindAsync(id);
            if (analytique == null)
            {
                return NotFound();
            }
            return View(analytique);
        }

        // POST: Analytiques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,precison")] Analytique analytique)
        {
            if (id != analytique.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analytique);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalytiqueExists(analytique.id))
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
            return View(analytique);
        }

        // GET: Analytiques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Analytique == null)
            {
                return NotFound();
            }

            var analytique = await _context.Analytique
                .FirstOrDefaultAsync(m => m.id == id);
            if (analytique == null)
            {
                return NotFound();
            }

            return View(analytique);
        }

        // POST: Analytiques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Analytique == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Analytique'  is null.");
            }
            var analytique = await _context.Analytique.FindAsync(id);
            if (analytique != null)
            {
                _context.Analytique.Remove(analytique);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalytiqueExists(int id)
        {
          return (_context.Analytique?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
