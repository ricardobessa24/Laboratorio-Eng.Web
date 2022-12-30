namespace Utad_Proj_.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Utad_Proj_.Data;
    using Utad_Proj_.Models;
    using Utad_Proj_.Services;
    using Utad_Proj_.ViewModel;

    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseService purchaseService;
        private readonly UserManager<ApplicationUser> userManager;

        public RoomsController(
            ApplicationDbContext context,
            IPurchaseService purchaseService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.purchaseService = purchaseService;
            this.userManager = userManager;
        }

        [HttpGet, ActionName("CheckOut")]
        [Route("CheckOut")]
        public async Task<IActionResult> CheckOut(
                    [FromQuery] int movieSessionId,
                    [FromQuery] int numberOfTickets,
                    [FromQuery] float price)
        {
            Movie_Session movieSession = await this._context.Sessions
                .Include(x => x.Movie)
                .Include(x => x.Room)
                .FirstOrDefaultAsync(x => x.Id == movieSessionId)
                .ConfigureAwait(false);

            string userId = this.userManager.GetUserId(this.User);

            PurchaseIntentViewModel purchaseIntent = new PurchaseIntentViewModel
            {
                ApplicationUserId = userId,
                MovieTitle = movieSession.Movie.Title,
                MovieSessionId = movieSessionId,
                NumberOfTickets = numberOfTickets,
                Price = price,
                RoomName = movieSession.Room.Name,
                Date = movieSession.Date_
            };
            return View(purchaseIntent);
        }

        [HttpPost, ActionName("ConfirmPurchase")]
        [Route("ConfirmPurchase")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPurchase(PurchaseIntentViewModel purchaseIntent)
        {
            await this.purchaseService.CreatePurchaseAsync(new CreatePurchaseArgs
            {
                ApplicationUserId = purchaseIntent.ApplicationUserId,
                Date = purchaseIntent.Date,
                MovieSessionId = purchaseIntent.MovieSessionId,
                Price = purchaseIntent.Price
            }).ConfigureAwait(false);

            return RedirectToAction("Index", "Movies");
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Num")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5 To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Num")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}