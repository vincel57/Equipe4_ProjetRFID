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
    public class HistoriquesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoriquesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Historiques
        public async Task<IActionResult> Index()
        {
              return _context.Historique != null ? 
                          View(await _context.Historique.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Historique'  is null.");
        }

        // GET: Historiques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Historique == null)
            {
                return NotFound();
            }

            var historique = await _context.Historique
                .FirstOrDefaultAsync(m => m.id == id);
            if (historique == null)
            {
                return NotFound();
            }

            return View(historique);
        }

        // GET: Historiques/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Historiques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,time_connex,UserId,UserName,RoleId")] Historique historique)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historique);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historique);
        }

        // GET: Historiques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Historique == null)
            {
                return NotFound();
            }

            var historique = await _context.Historique.FindAsync(id);
            if (historique == null)
            {
                return NotFound();
            }
            return View(historique);
        }

        // POST: Historiques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,time_connex,UserId,UserName,RoleId")] Historique historique)
        {
            if (id != historique.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historique);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoriqueExists(historique.id))
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
            return View(historique);
        }

        // GET: Historiques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Historique == null)
            {
                return NotFound();
            }

            var historique = await _context.Historique
                .FirstOrDefaultAsync(m => m.id == id);
            if (historique == null)
            {
                return NotFound();
            }

            return View(historique);
        }

        // POST: Historiques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Historique == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Historique'  is null.");
            }
            var historique = await _context.Historique.FindAsync(id);
            if (historique != null)
            {
                _context.Historique.Remove(historique);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoriqueExists(int id)
        {
          return (_context.Historique?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
