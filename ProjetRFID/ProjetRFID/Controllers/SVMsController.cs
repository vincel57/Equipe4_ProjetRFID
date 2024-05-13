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
    public class SVMsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SVMsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SVMs
        public async Task<IActionResult> Index()
        {
              return _context.SVM != null ? 
                          View(await _context.SVM.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SVM'  is null.");
        }

        // GET: SVMs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SVM == null)
            {
                return NotFound();
            }

            var sVM = await _context.SVM
                .FirstOrDefaultAsync(m => m.id == id);
            if (sVM == null)
            {
                return NotFound();
            }

            return View(sVM);
        }

        // GET: SVMs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SVMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,C,kernel,gamma,coef0,tol,cache_size,max_iter,precision")] SVM sVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sVM);
        }

        // GET: SVMs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SVM == null)
            {
                return NotFound();
            }

            var sVM = await _context.SVM.FindAsync(id);
            if (sVM == null)
            {
                return NotFound();
            }
            return View(sVM);
        }

        // POST: SVMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,C,kernel,gamma,coef0,tol,cache_size,max_iter,precision")] SVM sVM)
        {
            if (id != sVM.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SVMExists(sVM.id))
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
            return View(sVM);
        }

        // GET: SVMs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SVM == null)
            {
                return NotFound();
            }

            var sVM = await _context.SVM
                .FirstOrDefaultAsync(m => m.id == id);
            if (sVM == null)
            {
                return NotFound();
            }

            return View(sVM);
        }

        // POST: SVMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SVM == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SVM'  is null.");
            }
            var sVM = await _context.SVM.FindAsync(id);
            if (sVM != null)
            {
                _context.SVM.Remove(sVM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SVMExists(int id)
        {
          return (_context.SVM?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
