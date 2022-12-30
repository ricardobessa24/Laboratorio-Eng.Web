namespace Utad_Proj_.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Utad_Proj_.Data;
    using Utad_Proj_.Models;

    public class Movie_SessionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Movie_SessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet, ActionName("BeginTicketsPurchaseIntent")]
        public async Task<IActionResult> BeginTicketsPurchaseIntent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Sessions
                .Include(x => x.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Movie_Session/Create
        public IActionResult Create()
        {
            ViewData["MovieID"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["RoomID"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        // POST: Movie_Session/Create To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date_,MovieID,RoomID")] Movie_Session movie_Session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie_Session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movies, "Id", "Title", movie_Session.MovieID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "Id", "Name", movie_Session.RoomID);
            return View(movie_Session);
        }

        // GET: Movie_Session/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Session = await _context.Sessions
                .Include(m => m.Movie)
                .Include(m => m.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie_Session == null)
            {
                return NotFound();
            }

            return View(movie_Session);
        }

        // POST: Movie_Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie_Session = await _context.Sessions.FindAsync(id);
            _context.Sessions.Remove(movie_Session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Movie_Session/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Session = await _context.Sessions
                .Include(m => m.Movie)
                .Include(m => m.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie_Session == null)
            {
                return NotFound();
            }

            return View(movie_Session);
        }

        // GET: Movie_Session/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Session = await _context.Sessions.FindAsync(id);
            if (movie_Session == null)
            {
                return NotFound();
            }
            ViewData["MovieID"] = new SelectList(_context.Movies, "Id", "Title", movie_Session.MovieID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "Id", "Name", movie_Session.RoomID);
            return View(movie_Session);
        }

        // POST: Movie_Session/Edit/5 To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date_,MovieID,RoomID")] Movie_Session movie_Session)
        {
            if (id != movie_Session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie_Session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Movie_SessionExists(movie_Session.Id))
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
            ViewData["MovieID"] = new SelectList(_context.Movies, "Id", "Title", movie_Session.MovieID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "Id", "Name", movie_Session.RoomID);
            return View(movie_Session);
        }

        // GET: Movie_Session
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sessions.Include(m => m.Movie).Include(m => m.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        private bool Movie_SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}