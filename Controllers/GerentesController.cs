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
    public class GerentesController : Controller
    {
        private readonly BancoContext _context;

        public GerentesController(BancoContext context)
        {
            _context = context;
        }

        // GET: Gerentes
        public async Task<IActionResult> Index()
        {
            var bancoContext = _context.Gerentes.Include(g => g.NominaNavigation);
            return View(await bancoContext.ToListAsync());
        }

        // GET: Gerentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gerentes == null)
            {
                return NotFound();
            }

            var gerente = await _context.Gerentes
                .Include(g => g.NominaNavigation)
                .FirstOrDefaultAsync(m => m.NumNom == id);
            if (gerente == null)
            {
                return NotFound();
            }

            return View(gerente);
        }

        // GET: Gerentes/Create
        public IActionResult Create()
        {
            ViewData["Nomina"] = new SelectList(_context.Empleados, "Nomina", "Nomina");
            return View();
        }

        // POST: Gerentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumNom,Nomina,DiasVacaciones")] Gerente gerente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gerente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Nomina"] = new SelectList(_context.Empleados, "Nomina", "Nomina", gerente.Nomina);
            return View(gerente);
        }

        // GET: Gerentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gerentes == null)
            {
                return NotFound();
            }

            var gerente = await _context.Gerentes.FindAsync(id);
            if (gerente == null)
            {
                return NotFound();
            }
            ViewData["Nomina"] = new SelectList(_context.Empleados, "Nomina", "Nomina", gerente.Nomina);
            return View(gerente);
        }

        // POST: Gerentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumNom,Nomina,DiasVacaciones")] Gerente gerente)
        {
            if (id != gerente.NumNom)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gerente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenteExists(gerente.NumNom))
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
            ViewData["Nomina"] = new SelectList(_context.Empleados, "Nomina", "Nomina", gerente.Nomina);
            return View(gerente);
        }

        // GET: Gerentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Gerentes == null)
            {
                return NotFound();
            }

            var gerente = await _context.Gerentes
                .Include(g => g.NominaNavigation)
                .FirstOrDefaultAsync(m => m.NumNom == id);
            if (gerente == null)
            {
                return NotFound();
            }

            return View(gerente);
        }

        // POST: Gerentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gerentes == null)
            {
                return Problem("Entity set 'BancoContext.Gerentes'  is null.");
            }
            var gerente = await _context.Gerentes.FindAsync(id);
            if (gerente != null)
            {
                _context.Gerentes.Remove(gerente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GerenteExists(int id)
        {
          return _context.Gerentes.Any(e => e.NumNom == id);
        }
    }
}
