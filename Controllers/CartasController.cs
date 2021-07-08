using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CardCollector.Data;
using CardCollector.Models;
using Microsoft.AspNetCore.Authorization;
using CardCollector.Models.ViewModel;

namespace CardCollector.Controllers
{
    public class CartasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cartas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carta.ToListAsync());
        }

        // GET: Cartas/MostrarBusqueda
        public async Task<IActionResult> MostrarBusqueda()
        {
            return View();
        }

        // GET: Cartas/MostrarResultados
        public async Task<IActionResult> MostrarResultados(String Busqueda)
        {
            return View("Index", await _context.Carta.Where(c => c.Nombre.Contains(Busqueda)).ToListAsync());
        }
        // GET: Cartas/CartasPorUsuario
        public IActionResult CartasPorUsuario(int idUsuario)
        {
            var resul = from c in _context.Carta
                        join u in _context.Usuario on c.IdUser equals u.UserId
                        where (c.IdUser == idUsuario)
                        select new CardUserViewModel
                        {
                            idCard = c.Id,
                            userName = u.UserName,
                            cardName = c.Nombre,
                            description = c.Descripcion
                        };
            return View(resul);
        }

        // GET: Cartas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carta = (from c in _context.Carta
                        join u in _context.Usuario on c.IdUser equals u.UserId
                        where (c.Id == id)
                        select new CardUserViewModel
                        {
                            idCard = c.Id,
                            idUser = u.UserId,
                            userName = u.UserName,
                            cardName = c.Nombre,
                            description = c.Descripcion,
                            nivel = c.Nivel
                        }).FirstOrDefault();

            if (carta == null)
            {
                return NotFound();
    }

            return View(carta);
}

// GET: Cartas/Create
[Authorize]
public IActionResult Create()
{
    return View();
}

// POST: Cartas/Create
// To protect from overposting attacks, enable the specific properties you want to bind to, for 
// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Nivel")] Carta carta)
{
    if (ModelState.IsValid)
    {
        _context.Add(carta);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(carta);
}

// GET: Cartas/Edit/5
[Authorize]
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var carta = await _context.Carta.FindAsync(id);
    if (carta == null)
    {
        return NotFound();
    }
    return View(carta);
}

// POST: Cartas/Edit/5
// To protect from overposting attacks, enable the specific properties you want to bind to, for 
// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Nivel")] Carta carta)
{
    if (id != carta.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(carta);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CartaExists(carta.Id))
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
    return View(carta);
}

// GET: Cartas/Delete/5
[Authorize]
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var carta = await _context.Carta
        .FirstOrDefaultAsync(m => m.Id == id);
    if (carta == null)
    {
        return NotFound();
    }

    return View(carta);
}

// POST: Cartas/Delete/5
[Authorize]
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var carta = await _context.Carta.FindAsync(id);
    _context.Carta.Remove(carta);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

private bool CartaExists(int id)
{
    return _context.Carta.Any(e => e.Id == id);
}
    }
}
