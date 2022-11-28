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
    public class DatosPrestamoesController : Controller
    {
        private readonly BancoContext _context;

        public DatosPrestamoesController(BancoContext context)
        {
            _context = context;
        }

        // GET: DatosPrestamoes
        public async Task<IActionResult> Index()
        {
            var bancoContext = _context.DatosPrestamos.Include(d => d.NumHistorialNavigation);
            return View(await bancoContext.ToListAsync());
        }

        // GET: DatosPrestamoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DatosPrestamos == null)
            {
                return NotFound();
            }

            var datosPrestamo = await _context.DatosPrestamos
                .Include(d => d.NumHistorialNavigation)
                .FirstOrDefaultAsync(m => m.NumFolio == id);
            if (datosPrestamo == null)
            {
                return NotFound();
            }

            return View(datosPrestamo);
        }

        // GET: DatosPrestamoes/Create
        public IActionResult Create()
        {
            ViewData["NumHistorial"] = new SelectList(_context.Historials, "NumHistorial", "NumHistorial");
            return View();
        }

        // POST: DatosPrestamoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumFolio,FechaSolicitud,FechaAprobacion,FechaLiquidacion,FechaLimite,NumHistorial")] DatosPrestamo datosPrestamo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datosPrestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumHistorial"] = new SelectList(_context.Historials, "NumHistorial", "NumHistorial", datosPrestamo.NumHistorial);
            return View(datosPrestamo);
        }

        // GET: DatosPrestamoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatosPrestamos == null)
            {
                return NotFound();
            }

            var datosPrestamo = await _context.DatosPrestamos.FindAsync(id);
            if (datosPrestamo == null)
            {
                return NotFound();
            }
            ViewData["NumHistorial"] = new SelectList(_context.Historials, "NumHistorial", "NumHistorial", datosPrestamo.NumHistorial);
            return View(datosPrestamo);
        }

        // POST: DatosPrestamoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumFolio,FechaSolicitud,FechaAprobacion,FechaLiquidacion,FechaLimite,NumHistorial")] DatosPrestamo datosPrestamo)
        {
            if (id != datosPrestamo.NumFolio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datosPrestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatosPrestamoExists(datosPrestamo.NumFolio))
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
            ViewData["NumHistorial"] = new SelectList(_context.Historials, "NumHistorial", "NumHistorial", datosPrestamo.NumHistorial);
            return View(datosPrestamo);
        }

        // GET: DatosPrestamoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatosPrestamos == null)
            {
                return NotFound();
            }

            var datosPrestamo = await _context.DatosPrestamos
                .Include(d => d.NumHistorialNavigation)
                .FirstOrDefaultAsync(m => m.NumFolio == id);
            if (datosPrestamo == null)
            {
                return NotFound();
            }

            return View(datosPrestamo);
        }

        // POST: DatosPrestamoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatosPrestamos == null)
            {
                return Problem("Entity set 'BancoContext.DatosPrestamos'  is null.");
            }
            var datosPrestamo = await _context.DatosPrestamos.FindAsync(id);
            if (datosPrestamo != null)
            {
                _context.DatosPrestamos.Remove(datosPrestamo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatosPrestamoExists(int id)
        {
          return _context.DatosPrestamos.Any(e => e.NumFolio == id);
        }
    }
}
