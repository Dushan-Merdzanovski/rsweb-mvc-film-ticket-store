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
using MVCFilmTicketStore.ViewModels;

namespace MVCFilmTicketStore.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly MVCFilmTicketStoreContext _context;

        public DirectorsController(MVCFilmTicketStoreContext context)
        {
            _context = context;
        }

        // GET: Directors
        public async Task<IActionResult> Index(string? searchString)
        {
            IQueryable<Director> directors = _context.Director.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                directors = directors.Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString));
            }

            DirectorSearchViewModel viewmodel = new DirectorSearchViewModel
            {
                Directors = await directors.ToListAsync()
            };

            return View(viewmodel);
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Director == null)
            {
                return NotFound();
            }

            var director = await _context.Director
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate,Country,Gender,ProfilePictureUrl")] Director director)
        {
            if (ModelState.IsValid)
            {
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        // GET: Directors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Director == null)
            {
                return NotFound();
            }

            var director = await _context.Director.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,Country,Gender,ProfilePictureUrl")] Director director)
        {
            if (id != director.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.Id))
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
            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Director == null)
            {
                return NotFound();
            }

            var director = await _context.Director
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Director == null)
            {
                return Problem("Entity set 'MVCFilmTicketStoreContext.Director'  is null.");
            }
            var director = await _context.Director.FindAsync(id);
            if (director != null)
            {
                _context.Director.Remove(director);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
          return (_context.Director?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
