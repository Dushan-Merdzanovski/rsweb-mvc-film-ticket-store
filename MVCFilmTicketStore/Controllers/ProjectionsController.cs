using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFilmTicketStore.Data;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.Controllers
{
    public class ProjectionsController : Controller
    {
        private readonly MVCFilmTicketStoreContext _context;

        public ProjectionsController(MVCFilmTicketStoreContext context)
        {
            _context = context;
        }

        // GET: Projections
        public async Task<IActionResult> Index()
        {
            var mVCFilmTicketStoreContext = _context.Projection.Include(p => p.Film).Include(p => p.Theater);
            return View(await mVCFilmTicketStoreContext.ToListAsync());
        }

        // GET: Projections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projection == null)
            {
                return NotFound();
            }

            var projection = await _context.Projection
                .Include(p => p.Film)
                .Include(p => p.Theater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projection == null)
            {
                return NotFound();
            }

            return View(projection);
        }

        // GET: Projections/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title");
            ViewData["TheaterId"] = new SelectList(_context.Theater, "Id", "Name");
            return View();
        }

        // POST: Projections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FreeSeatsNum,ProjectionTime,Price,Is3D,FilmId,TheaterId")] Projection projection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", projection.FilmId);
            ViewData["TheaterId"] = new SelectList(_context.Theater, "Id", "Name", projection.TheaterId);
            return View(projection);
        }

        // GET: Projections/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projection == null)
            {
                return NotFound();
            }

            var projection = await _context.Projection.FindAsync(id);
            if (projection == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", projection.FilmId);
            ViewData["TheaterId"] = new SelectList(_context.Theater, "Id", "Name", projection.TheaterId);
            return View(projection);
        }

        // POST: Projections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FreeSeatsNum,ProjectionTime,Price,Is3D,FilmId,TheaterId")] Projection projection)
        {
            if (id != projection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectionExists(projection.Id))
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
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", projection.FilmId);
            ViewData["TheaterId"] = new SelectList(_context.Theater, "Id", "Name", projection.TheaterId);
            return View(projection);
        }

        // GET: Projections/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projection == null)
            {
                return NotFound();
            }

            var projection = await _context.Projection
                .Include(p => p.Film)
                .Include(p => p.Theater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projection == null)
            {
                return NotFound();
            }

            return View(projection);
        }

        // POST: Projections/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projection == null)
            {
                return Problem("Entity set 'MVCFilmTicketStoreContext.Projection'  is null.");
            }
            var projection = await _context.Projection.FindAsync(id);
            if (projection != null)
            {
                _context.Projection.Remove(projection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectionExists(int id)
        {
          return (_context.Projection?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
