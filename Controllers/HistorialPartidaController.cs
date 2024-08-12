using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Preguntado.Models;

namespace Preguntado.Controllers
{
    public class HistorialPartidaController : Controller
    {
        private readonly WebPreguntadosContext _context;

        public HistorialPartidaController(WebPreguntadosContext context)
        {
            _context = context;
        }

        // GET: HistorialPartidums
        public async Task<IActionResult> Index()
        {
            var webPreguntadosContext = _context.HistorialPartida.Include(h => h.IdJugadorNavigation);
            return View(await webPreguntadosContext.ToListAsync());
        }

        // POST: HistorialPartida/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HistorialPartidum historialPartidum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HistorialPartida.Add(historialPartidum);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

