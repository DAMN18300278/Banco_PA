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
    public class HistorialsController : Controller
    {
        private readonly BancoContext _context;

        public HistorialsController(BancoContext context)
        {
            _context = context;
        }

        // GET: Historials
        public async Task<IActionResult> Index()
        {
            var bancoContext = _context.Historials.Include(h => h.NominaEncargadoNavigation).Include(h => h.NumCuentaNavigation);
            return View(await bancoContext.ToListAsync());
        }

        // GET: Historials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Historials == null)
            {
                return NotFound();
            }

            var historial = await _context.Historials
                .Include(h => h.NominaEncargadoNavigation)
                .Include(h => h.NumCuentaNavigation)
                .FirstOrDefaultAsync(m => m.NumHistorial == id);
            if (historial == null)
            {
                return NotFound();
            }

            return View(historial);
        }

        // GET: Historials/Create
        public IActionResult Create()
        {
            ViewData["NominaEncargado"] = new SelectList(_context.Empleados, "Nomina", "Nomina");
            ViewData["NumCuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta");
            return View();
        }

        // POST: Historials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumHistorial,Cantidad,PagoRealizados,PagoPendientes,NumCuenta,NominaEncargado,Estado")] Historial historial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NominaEncargado"] = new SelectList(_context.Empleados, "Nomina", "Nomina", historial.NominaEncargado);
            ViewData["NumCuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta", historial.NumCuenta);
            return View(historial);
        }

        // GET: Historials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Historials == null)
            {
                return NotFound();
            }

            var historial = await _context.Historials.FindAsync(id);
            if (historial == null)
            {
                return NotFound();
            }
            ViewData["NominaEncargado"] = new SelectList(_context.Empleados, "Nomina", "Nomina", historial.NominaEncargado);
            ViewData["NumCuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta", historial.NumCuenta);
            return View(historial);
        }

        // POST: Historials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumHistorial,Cantidad,PagoRealizados,PagoPendientes,NumCuenta,NominaEncargado,Estado")] Historial historial)
        {
            if (id != historial.NumHistorial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistorialExists(historial.NumHistorial))
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
            ViewData["NominaEncargado"] = new SelectList(_context.Empleados, "Nomina", "Nomina", historial.NominaEncargado);
            ViewData["NumCuenta"] = new SelectList(_context.Cuenta, "NumCuenta", "NumCuenta", historial.NumCuenta);
            return View(historial);
        }

        // GET: Historials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Historials == null)
            {
                return NotFound();
            }

            var historial = await _context.Historials
                .Include(h => h.NominaEncargadoNavigation)
                .Include(h => h.NumCuentaNavigation)
                .FirstOrDefaultAsync(m => m.NumHistorial == id);
            if (historial == null)
            {
                return NotFound();
            }

            return View(historial);
        }

        // POST: Historials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Historials == null)
            {
                return Problem("Entity set 'BancoContext.Historials'  is null.");
            }
            var historial = await _context.Historials.FindAsync(id);
            if (historial != null)
            {
                _context.Historials.Remove(historial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistorialExists(int id)
        {
          return _context.Historials.Any(e => e.NumHistorial == id);
        }
    }
}
