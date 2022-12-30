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
    using Utad_Proj_.ViewModel;

    public class ArticleCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ArticleCommentViewModel vm)
        {
            var comment = vm.Comment;
            var articleId = vm.ArticlesId;
            var rating = vm.Rating;

            ArticleComment artComment = new ArticleComment()
            {
                ArticleId = articleId,
                Comments = comment,
                Rating = rating,
                PublishDate = DateTime.Now
            };

            _context.ArticlesComments.Add(artComment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Articles", new { id = articleId });
        }

        // GET: ArticleComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArticleComments/Create To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comments,PublishDate,ArticlesId,Rating")] ArticleComment articleComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articleComment);
        }

        // GET: ArticleComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleComment = await _context.ArticlesComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleComment == null)
            {
                return NotFound();
            }

            return View(articleComment);
        }

        // POST: ArticleComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleComment = await _context.ArticlesComments.FindAsync(id);
            _context.ArticlesComments.Remove(articleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ArticleComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleComment = await _context.ArticlesComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleComment == null)
            {
                return NotFound();
            }

            return View(articleComment);
        }

        // GET: ArticleComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleComment = await _context.ArticlesComments.FindAsync(id);
            if (articleComment == null)
            {
                return NotFound();
            }
            return View(articleComment);
        }

        // POST: ArticleComments/Edit/5 To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comments,PublishDate,ArticlesId,Rating")] ArticleComment articleComment)
        {
            if (id != articleComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleCommentExists(articleComment.Id))
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
            return View(articleComment);
        }

        // GET: ArticleComments
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticlesComments.ToListAsync());
        }

        private bool ArticleCommentExists(int id)
        {
            return _context.ArticlesComments.Any(e => e.Id == id);
        }
    }
}