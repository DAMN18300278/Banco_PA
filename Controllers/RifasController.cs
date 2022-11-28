using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Banco.Models;

namespace Banco.Controllers
{
    public class RifasController : Controller
    {
        private readonly BancoContext _context;

        public RifasController(BancoContext context)
        {
            _context = context;
        }

        // GET: Rifas
        public async Task<IActionResult> Index()
        {
            var bancoContext = _context.Rifas.Include(r => r.CuentaNavigation);
            return View(await bancoContext.ToListAsync());
        }

        // GET: Rifas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rifas == null)
            {
                return NotFound();
            }

            var rifa = await _context.Rifas
                .Include(r => r.CuentaNavigation)
                .FirstOrDefaultAsync(m => m.NumBoleto == id);
            if (rifa == null)
            {
                return NotFound();
            }

            return View(rifa);
        }

        // GET: Rifas/Create
        public IActionResult Create()
        {
            ViewData["Cuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta");
            return View();
        }

        // POST: Rifas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumBoleto,Cuenta,FechaBoleto")] Rifa rifa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rifa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta", rifa.Cuenta);
            return View(rifa);
        }

        // GET: Rifas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rifas == null)
            {
                return NotFound();
            }

            var rifa = await _context.Rifas.FindAsync(id);
            if (rifa == null)
            {
                return NotFound();
            }
            ViewData["Cuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta", rifa.Cuenta);
            return View(rifa);
        }

        // POST: Rifas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumBoleto,Cuenta,FechaBoleto")] Rifa rifa)
        {
            if (id != rifa.NumBoleto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rifa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RifaExists(rifa.NumBoleto))
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
            ViewData["Cuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta", rifa.Cuenta);
            return View(rifa);
        }

        // GET: Rifas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rifas == null)
            {
                return NotFound();
            }

            var rifa = await _context.Rifas
                .Include(r => r.CuentaNavigation)
                .FirstOrDefaultAsync(m => m.NumBoleto == id);
            if (rifa == null)
            {
                return NotFound();
            }

            return View(rifa);
        }

        // POST: Rifas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rifas == null)
            {
                return Problem("Entity set 'BancoContext.Rifas'  is null.");
            }
            var rifa = await _context.Rifas.FindAsync(id);
            if (rifa != null)
            {
                _context.Rifas.Remove(rifa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RifaExists(int id)
        {
          return _context.Rifas.Any(e => e.NumBoleto == id);
        }
    }
}
