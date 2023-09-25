using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFilmTicketStore.Areas.Identity.Data;
using MVCFilmTicketStore.Data;
using MVCFilmTicketStore.DataTypes.Enums;
using MVCFilmTicketStore.Interfaces;
using MVCFilmTicketStore.Models;
using MVCFilmTicketStore.ViewModels;

namespace MVCFilmTicketStore.Controllers
{
    public class TicketsController : Controller
    {
        private readonly MVCFilmTicketStoreContext _context;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<MVCFilmTicketStoreUser> _usermanager;

        public TicketsController(
            MVCFilmTicketStoreContext context,
            IBufferedFileUploadService bufferedFileUploadService,
            IWebHostEnvironment webHostEnvironment,
            UserManager<MVCFilmTicketStoreUser> usermanager)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
            _webHostEnvironment = webHostEnvironment;
            _usermanager = usermanager;
        }

        private Task<MVCFilmTicketStoreUser> GetCurrentUserAsync() => _usermanager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> DownloadFile(string downloadUrl, FolderType folder)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "files_images", folder.ToString(), downloadUrl);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", downloadUrl);
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var mVCFilmTicketStoreContext = _context.Ticket.Include(t => t.Projection).ThenInclude(p => p.Film).Include(p => p.TicketSeats);
            return View(await mVCFilmTicketStoreContext.ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyTicketsList()
        {
            var user = await GetCurrentUserAsync();
            var ticketsContext = _context.Ticket.AsQueryable().Where(r => r.AppUser == user.UserName).Include(t => t.Projection).ThenInclude(p => p.Film).Include(p => p.TicketSeats).AsEnumerable();
            return ticketsContext != null ?
                          View("~/Views/Tickets/MyTickets.cshtml", ticketsContext) :
                          Problem("Entity set 'MVCBookStoreContext.UserBook'  is null.");
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Projection).ThenInclude(p => p.Film)
                .Include(p => p.TicketSeats)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET
        /*        [Authorize(Roles = "User")]*/
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> BuyTicketGet(int? projectionId)
        {
            var projection =
                _context.Projection.AsQueryable().Where(p => p.Id == projectionId)
                .Include(p => p.Theater)
                .Include(p => p.Film);

            BuyTicketViewModel viewmodel = new BuyTicketViewModel
            {
                Projection = await projection.FirstOrDefaultAsync()
            };
            return View("~/Views/Tickets/BuyTicket.cshtml", viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> BuyTicket(BuyTicketViewModel viewmodel)
        {
            MVCFilmTicketStoreUser user = await GetCurrentUserAsync();
            viewmodel.NewTicket.AppUser = user.UserName;

            if (ModelState.IsValid)
            {
                viewmodel.NewTicket.BoughtTime = DateTime.Now;
                viewmodel.NewTicket.IsValid = true;
                _context.Add(viewmodel.NewTicket);
                await _context.SaveChangesAsync();

                return RedirectToAction("ReserveSeatsGet", new { projectionId = viewmodel.NewTicket.ProjectionId, ticketId = viewmodel.NewTicket.Id.ToString() });
            }

            //return View(viewmodel);
            return RedirectToAction("FilmsProgram", "Films");
        }

        // GET
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ReserveSeatsGet(int? projectionId, int? ticketId)
        {
            var projection =
                _context.Projection.AsQueryable().Where(p => p.Id == projectionId)
                .Include(p => p.Theater)
                .Include(p => p.Film)
                .Include(p => p.Tickets).ThenInclude(p => p.TicketSeats).ThenInclude(p => p.Seat).FirstOrDefault();

            List<Seat> takenSeats = projection.Tickets.SelectMany(ticket => ticket.TicketSeats).Select(p => p.Seat).ToList();

            List<Seat> allSeatsInDb = _context.Seat.ToList();

            var newTicket = projection.Tickets.Where(p => p.Id == ticketId).FirstOrDefault();

            ReserveSeatsViewModel viewmodel = new ReserveSeatsViewModel
            {
                Projection = projection,
                TakenSeats = takenSeats,
                AllSeats = allSeatsInDb,
                NewTicket = newTicket
            };
            return View("~/Views/Tickets/ReserveSeats.cshtml", viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ReserveSeats(int[] newSeatIDs, int projectionId, int ticketId)
        {
            if (ModelState.IsValid && newSeatIDs != null && projectionId != null && ticketId != null)
            {
                foreach (int seatId in newSeatIDs)
                {
                    _context.TicketSeat.Add(new TicketSeat { TicketId = ticketId, SeatId = seatId });
                }
                await _context.SaveChangesAsync();

                var projection = _context.Projection.Where(p => p.Id == projectionId).FirstOrDefault();

                projection.FreeSeatsNum = projection.FreeSeatsNum - newSeatIDs.Length;

                _context.Update(projection);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            //return View(viewmodel);
            return RedirectToAction("FilmsProgram", "Films");
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProjectionId"] = new SelectList(_context.Projection, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BoughtTime,DownloadTicketUrl,IsValid,AppUser,ProjectionId")] Ticket ticket, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();

                // FILE UPLOAD
                if (file != null)
                {
                    string newImagePath = await _bufferedFileUploadService.UploadFile(file, _webHostEnvironment, FolderType.TICKETS);
                    if (newImagePath != null)
                    {
                        ViewBag.Message = "File Upload Successful";
                    }
                    else
                    {
                        ViewBag.Message = "File Upload Failed";
                    }
                    ticket.DownloadTicketUrl = newImagePath;
                    _context.Update(ticket);
                }
                //END FILE UPLOAD
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectionId"] = new SelectList(_context.Projection, "Id", "Id", ticket.ProjectionId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ProjectionId"] = new SelectList(_context.Projection, "Id", "Id", ticket.ProjectionId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BoughtTime,DownloadTicketUrl,IsValid,AppUser,ProjectionId")] Ticket ticket, IFormFile file)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string newImagePath = await _bufferedFileUploadService.UploadFile(file, _webHostEnvironment, FolderType.TICKETS);
                        if (newImagePath != null)
                        {
                            ViewBag.Message = "File Upload Successful";
                        }
                        else
                        {
                            ViewBag.Message = "File Upload Failed";
                        }
                        ticket.DownloadTicketUrl = newImagePath;
                        _context.Update(ticket);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["ProjectionId"] = new SelectList(_context.Projection, "Id", "Id", ticket.ProjectionId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Projection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'MVCFilmTicketStoreContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return (_context.Ticket?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
