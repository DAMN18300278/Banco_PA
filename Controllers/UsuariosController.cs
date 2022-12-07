using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Banco.Models;
using Microsoft.AspNetCore.Session;


namespace Banco.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly BancoContext _context;

        public UsuariosController(BancoContext context)
        {
            _context = context;
        }

        public IActionResult ValidationUser(){
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidationUser([Bind("Nom_Usuario,Contraseña")] Usuario usuario){
            Usuario usuarioValidar = await _context.Usuarios.FindAsync(_context.Usuarios.Where(b => b.Nom_Usuario.Equals(usuario.Nom_Usuario)).FirstOrDefault()?.Curp ?? "p");

            if(usuarioValidar != null)
            {
                if(usuario.Contraseña.Equals(usuarioValidar.Contraseña) && usuarioValidar.Autorizada.Equals(2)){
                    HttpContext.Session.SetString("User", usuarioValidar.Curp);

                    if(_context.Empleados.Any(e => e.Curp == usuarioValidar.Curp))
                    {
                        if(_context.Gerentes.Where(b => b.Nomina == _context.Empleados.Where(b => b.Curp == usuarioValidar.Curp).FirstOrDefault().Nomina) != null)
                        {
                            return RedirectToAction("Session", "Gerentes");
                        }else{
                            return RedirectToAction("Session", "Empleadoes");
                        }
                    }
                    return RedirectToAction("Session", "Cuenta");

                }else{
                    return RedirectToAction(nameof(Login));
                }
            }

            return RedirectToAction(nameof(Login));
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Usuarios.ToListAsync());
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
                
        public async Task<IActionResult> AdminUsers()
        {
            Usuario nombreSession = _context.Usuarios.Find(HttpContext.Session.GetString("User"));
            if(nombreSession != null)
            {
                ViewData["User"] = nombreSession.NombreS;
            }
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Usuario nombreSession = _context.Usuarios.Find(HttpContext.Session.GetString("User"));
            if(nombreSession != null)
            {
                ViewData["User"] = nombreSession.NombreS;
            }
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Curp == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Curp,NombreS,ApellidoP,ApellidoM,Cumpleaños,Contraseña")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            
            ViewData["Auth"] = new SelectList(new List<SelectListItem>{
            new SelectListItem { Selected = false, Text = "Rechazado", Value = "1"},
            new SelectListItem { Selected = false, Text = "Aceptado", Value = "2"},
            new SelectListItem { Selected = false, Text = "Pendiente", Value = "0"}
            }, "Value", "Text");

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Curp,NombreS,ApellidoP,ApellidoM,Cumpleaños,Autorizada")] Usuario usuario)
        {
            var curp = usuario.Curp;

            Usuario? tempUser = _context.Usuarios.Find(curp);

            if(!tempUser.Autorizada.Equals(2) && usuario.Autorizada.Equals(2) && tempUser.Nom_Usuario == null)
            {
                int num_Nombre = 1;
                usuario.Nom_Usuario = usuario.Curp.Substring(0, 4) + num_Nombre.ToString();

                while(_context.Usuarios.Where(d => d.Nom_Usuario == usuario.Nom_Usuario).FirstOrDefault() != null)
                {
                    num_Nombre ++;
                    usuario.Nom_Usuario = usuario.Curp.Substring(0, 4) + num_Nombre.ToString();
                }

                usuario.Contraseña = usuario.Cumpleaños.Day.ToString() + "-" + usuario.Cumpleaños.Month.ToString() + "-" + usuario.Cumpleaños.Year.ToString();
            }

            if (id != usuario.Curp)
            {
                return NotFound();
            }

            _context.ChangeTracker.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Curp))
                    {
                        return RedirectToAction(nameof(AdminUsers));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(AdminUsers));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Curp == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'BancoContext.Usuarios' is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
          return _context.Usuarios.Any(e => e.Curp == id);
        }
    }
}
