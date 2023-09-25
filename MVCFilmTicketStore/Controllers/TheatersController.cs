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
    public class TheatersController : Controller
    {
        private readonly MVCFilmTicketStoreContext _context;

        public TheatersController(MVCFilmTicketStoreContext context)
        {
            _context = context;
        }

        // GET: Theaters
        public async Task<IActionResult> Index()
        {
              return _context.Theater != null ? 
                          View(await _context.Theater.ToListAsync()) :
                          Problem("Entity set 'MVCFilmTicketStoreContext.Theater'  is null.");
        }

        // GET: Theaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Theater == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // GET: Theaters/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Theaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AllSeatsNum,AllRows,AllColumns,TheaterPicture")] Theater theater)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theater);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theater);
        }

        // GET: Theaters/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Theater == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }
            return View(theater);
        }

        // POST: Theaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AllSeatsNum,AllRows,AllColumns,TheaterPicture")] Theater theater)
        {
            if (id != theater.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theater);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheaterExists(theater.Id))
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
            return View(theater);
        }

        // GET: Theaters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Theater == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // POST: Theaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Theater == null)
            {
                return Problem("Entity set 'MVCFilmTicketStoreContext.Theater'  is null.");
            }
            var theater = await _context.Theater.FindAsync(id);
            if (theater != null)
            {
                _context.Theater.Remove(theater);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheaterExists(int id)
        {
          return (_context.Theater?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
