using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preguntado.Models;

namespace Preguntado.Controllers
{
    public class JugadorController : Controller
    {
        private readonly WebPreguntadosContext _context;

        public JugadorController(WebPreguntadosContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Register([Bind("Nombre,Contraseña")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jugador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Jugador"); 
            }
            return View(jugador); 
        }

        public async Task<IActionResult> Login([Bind("Nombre,Contraseña")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                var existingJugador = await _context.Jugadors
                    .FirstOrDefaultAsync(j => j.Nombre == jugador.Nombre && j.Contraseña == jugador.Contraseña);

                if (existingJugador == null)
                {
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                    return View(jugador);
                }

                // Crear la identidad del usuario
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingJugador.Nombre),
                new Claim(ClaimTypes.NameIdentifier, existingJugador.IdJugador.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            return View(jugador);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // GET: Jugador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadors
                .FirstOrDefaultAsync(m => m.IdJugador == id);
            if (jugador == null)
            {
                return NotFound();
            }

            return View(jugador);
        }

        private bool JugadorExists(int id)
        {
            return _context.Jugadors.Any(e => e.IdJugador == id);
        }
    }
}
