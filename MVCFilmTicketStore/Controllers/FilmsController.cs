using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFilmTicketStore.Data;
using MVCFilmTicketStore.Models;
using MVCFilmTicketStore.ViewModels;
using static MVCFilmTicketStore.ViewModels.FilmActorsGenresEditViewModel;

namespace MVCFilmTicketStore.Controllers
{
    public class FilmsController : Controller
    {
        private readonly MVCFilmTicketStoreContext _context;

        public FilmsController(MVCFilmTicketStoreContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var mVCFilmTicketStoreContext = _context.Film.Include(f => f.Director).Include(p => p.ActorFilms).ThenInclude(p => p.Actor);
            return View(await mVCFilmTicketStoreContext.ToListAsync());
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Film == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .Include(f => f.Director)
                .Include(p => p.ActorFilms).ThenInclude(p => p.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            IEnumerable<Actor> actors = _context.Actor.AsEnumerable();
            actors = actors.OrderBy(s => s.FullName);

            IEnumerable<Genre> genres = _context.Genre.AsEnumerable();
            genres = genres.OrderBy(s => s.GenreName);

            FilmActorsGenresEditViewModel viewmodel = new FilmActorsGenresEditViewModel
            {
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                ActorList = new MultiSelectList(actors, "Id", "FullName")
            };

            ViewData["DirectorId"] = new SelectList(_context.Director, "Id", "FullName");

            return View(viewmodel);
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmActorsGenresEditViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewmodel.Film);
                await _context.SaveChangesAsync();

                IEnumerable<int> selectedGenreList = viewmodel.SelectedGenres;
                if (selectedGenreList != null)
                {
                    foreach (int genreId in selectedGenreList)
                    {
                        _context.FilmGenre.Add(new FilmGenre { GenreId = genreId, FilmId = viewmodel.Film.Id });

                    }
                }
                await _context.SaveChangesAsync();

                IEnumerable<int> selectedActorList = viewmodel.SelectedActors;
                if (selectedActorList != null)
                {
                    foreach (int actorId in selectedActorList)
                    {
                        _context.ActorFilm.Add(new ActorFilm { ActorId = actorId, FilmId = viewmodel.Film.Id });

                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Director, "Id", "FullName", viewmodel.Film.DirectorId);
            return View(viewmodel);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Film == null)
            {
                return NotFound();
            }

            var film = await _context.Film.Where(m => m.Id == id).Include(m => m.ActorFilms).Include(p => p.FilmGenres).FirstOrDefaultAsync();

            if (film == null)
            {
                return NotFound();
            }

            var actors = _context.Actor.AsEnumerable();
            actors = actors.OrderBy(s => s.FullName);

            var genres = _context.Genre.AsEnumerable();
            genres = genres.OrderBy(s => s.GenreName);

            FilmActorsGenresEditViewModel viewmodel = new FilmActorsGenresEditViewModel
            {
                Film = film,
                ActorList = new MultiSelectList(actors, "Id", "FullName"),
                SelectedActors = film.ActorFilms.Select(sa => sa.ActorId),
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenres = film.FilmGenres.Select(genre => genre.GenreId)
            };

            ViewData["DirectorId"] = new SelectList(_context.Director, "Id", "FullName", film.DirectorId);
            return View(viewmodel);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FilmActorsGenresEditViewModel viewmodel)
        {
            if (id != viewmodel.Film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Film);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> newActorList = viewmodel.SelectedActors;
                    IEnumerable<int> prevActorList = _context.ActorFilm.Where(s => s.FilmId == id).Select(s => s.ActorId);
                    IQueryable<ActorFilm> toBeRemoved = _context.ActorFilm.Where(s => s.FilmId == id);
                    if (newActorList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newActorList.Contains(s.ActorId));
                        foreach (int actorId in newActorList)
                        {
                            if (!prevActorList.Any(s => s == actorId))
                            {
                                _context.ActorFilm.Add(new ActorFilm { ActorId = actorId, FilmId = id });
                            }
                        }
                    }
                    _context.ActorFilm.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();

                    UpdateGenres(id, viewmodel, _context);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(viewmodel.Film.Id))
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
            ViewData["DirectorId"] = new SelectList(_context.Director, "Id", "FullName", viewmodel.Film.DirectorId);
            return View(viewmodel);
        }

        private static void UpdateGenres(int id, FilmActorsGenresEditViewModel viewmodel, MVCFilmTicketStoreContext _context)
        {
            IEnumerable<int> newGenreList = viewmodel.SelectedGenres;
            IEnumerable<int> prevGenreList = _context.FilmGenre.Where(s => s.FilmId == id).Select(s => s.GenreId);
            IQueryable<FilmGenre> toBeRemoved = _context.FilmGenre.Where(s => s.FilmId == id);
            if (newGenreList != null)
            {
                toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                foreach (int genreId in newGenreList)
                {
                    if (!prevGenreList.Any(s => s == genreId))
                    {
                        _context.FilmGenre.Add(new FilmGenre { GenreId = genreId, FilmId = id });
                    }
                }
            }
            _context.FilmGenre.RemoveRange(toBeRemoved);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Film == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .Include(f => f.Director)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Film == null)
            {
                return Problem("Entity set 'MVCFilmTicketStoreContext.Film'  is null.");
            }
            var film = await _context.Film.FindAsync(id);
            if (film != null)
            {
                _context.Film.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return (_context.Film?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
