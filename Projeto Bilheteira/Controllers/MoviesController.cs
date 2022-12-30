namespace Utad_Proj_.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Utad_Proj_.Data;
    using Utad_Proj_.Models;
    using Utad_Proj_.Services;

    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSenderService emailSenderService;
        private readonly NewMovieEmailSenderOptions newMovieEmailSenderOptions;

        public MoviesController(ApplicationDbContext context,
            IEmailSenderService emailSenderService,
            NewMovieEmailSenderOptions newMovieEmailSenderOptions)
        {
            _context = context;
            this.emailSenderService = emailSenderService;
            this.newMovieEmailSenderOptions = newMovieEmailSenderOptions;
        }

        public static List<SelectListItem> GetDropDownListForYears()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            for (int i = 1920; i <= DateTime.Now.Year; i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ls;
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Movies/Create To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,year,description,trailer,cast,photo,CategoryID")] Movie movie)
        {
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    movie.photo = dataStream.ToArray();
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                int affectedRecords = await _context.SaveChangesAsync().ConfigureAwait(false);

                if (affectedRecords > 0)
                {
                    var allUsers = await this._context.Users.ToListAsync().ConfigureAwait(false);
                    var category = await this._context.Categories.FirstOrDefaultAsync(x => x.Id == movie.CategoryID).ConfigureAwait(false);

                    var contentBuilder = new StringBuilder();
                    contentBuilder.AppendLine("New movie released:")
                        .AppendLine()
                        .AppendLine($"* Title: {movie.Title}")
                        .AppendLine($"* Year: {movie.year}")
                        .AppendLine($"* Category: {category.Name}")
                        .AppendLine()
                        .AppendLine("Thank you.");
                    string content = contentBuilder.ToString();

                    foreach (var user in allUsers)
                    {
                        await this.emailSenderService.SendEmailAsync(new SendEmailArgs
                        {
                            HtmlContent = content,
                            ReceiverEmail = user.Email,
                            ReceiverName = user.UserName,
                            SenderEmail = this.newMovieEmailSenderOptions.SenderEmail,
                            SenderName = this.newMovieEmailSenderOptions.SenderName,
                            Subject = "New movie released"
                        }).ConfigureAwait(false);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", movie.CategoryID);
            return View(movie);
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Delete/5
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var moviemodel = new MovieEdit();
            moviemodel.Id = movie.Id;
            moviemodel.Title = movie.Title;
            moviemodel.description = movie.description;
            moviemodel.year = movie.year;
            moviemodel.photo = movie.photo;
            moviemodel.cast = movie.cast;
            moviemodel.trailer = movie.trailer;
            moviemodel.CategoryID = movie.CategoryID;
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", movie.CategoryID);
            return View(movie);
        }

        // POST: Movies/Edit/5 To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieEdit model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    model.photo = dataStream.ToArray();
                }
            }
            var movie = await _context.Movies.FindAsync(id);
            movie.Title = model.Title;
            movie.year = model.year;
            if (model.photo != null)
                movie.photo = model.photo;
            movie.cast = model.cast;
            movie.CategoryID = model.CategoryID;
            movie.description = model.description;
            movie.trailer = model.trailer;
            movie.cast = model.cast;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", movie.CategoryID);
            return View(movie);
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Movies.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}