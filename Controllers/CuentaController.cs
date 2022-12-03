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
    public class CuentaController : Controller
    {
        private readonly BancoContext _context;

        public CuentaController(BancoContext context)
        {
            _context = context;
        }

        public ActionResult Session()
        {
            if(TempData["User"] != null)
            {
                ViewBag.User = TempData["User"].ToString();
            }
            return View();
        }

        // GET: Cuenta
        public async Task<IActionResult> Index()
        {
            var bancoContext = _context.Cuenta.Include(c => c.UsuarioNavigation);
            return View(await bancoContext.ToListAsync());
        }

        // GET: Cuenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cuenta == null)
            {
                return NotFound();
            }

            var cuentum = await _context.Cuenta
                .Include(c => c.UsuarioNavigation)
                .FirstOrDefaultAsync(m => m.NumCuenta == id);
            if (cuentum == null)
            {
                return NotFound();
            }

            return View(cuentum);
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Curp", "Curp");
            return View();
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuario,Saldo")] Cuentum cuentum)
        {
            if (ModelState.IsValid)
            {
                bool state = false;
                foreach (Cuentum item in _context.Cuenta)
                {
                    if(item.Usuario.Equals(cuentum.Usuario)){
                        state = true;
                    }
                }
                if(state){
                    return RedirectToAction(nameof(Index));
                }else{
                    _context.Add(cuentum);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Curp", "Curp", cuentum.Usuario);
            return View(cuentum);
        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cuenta == null)
            {
                return NotFound();
            }

            var cuentum = await _context.Cuenta.FindAsync(id);
            if (cuentum == null)
            {
                return NotFound();
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Curp", "Curp", cuentum.Usuario);
            return View(cuentum);
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumCuenta,Usuario,Saldo")] Cuentum cuentum)
        {
            if (id != cuentum.NumCuenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuentum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentumExists(cuentum.NumCuenta))
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
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Curp", "Curp", cuentum.Usuario);
            return View(cuentum);
        }

        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cuenta == null)
            {
                return NotFound();
            }

            var cuentum = await _context.Cuenta
                .Include(c => c.UsuarioNavigation)
                .FirstOrDefaultAsync(m => m.NumCuenta == id);
            if (cuentum == null)
            {
                return NotFound();
            }

            return View(cuentum);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cuenta == null)
            {
                return Problem("Entity set 'BancoContext.Cuenta'  is null.");
            }
            var cuentum = await _context.Cuenta.FindAsync(id);
            if (cuentum != null)
            {
                _context.Cuenta.Remove(cuentum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentumExists(int id)
        {
          return _context.Cuenta.Any(e => e.NumCuenta == id);
        }
    }
}
