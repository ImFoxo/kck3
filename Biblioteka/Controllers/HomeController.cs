using Biblioteka.Context;
using Biblioteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Biblioteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly BibContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BibContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (_context.AdminSettings.Find(1) == null)
            {
                var adminSettings = new AdminSettings();

                adminSettings.limitTaken = 6;
                adminSettings.limitTimeTaken = 4;
                adminSettings.limitWaiting = 6;
                adminSettings.limitTimeWaiting = 1;

                _context.Add(adminSettings);
                await _context.SaveChangesAsync();               
            }
            var user = _context.Readers.FirstOrDefault(r => r.email == User.Identity.Name);
            ViewData["userId"] = user == null ? null : user.id;

            ViewBag.BooksList = await _context.Book.ToListAsync();
            var bibContext = _context.Books.Include(b => b.category).Include(b => b.authors).Include(b => b.tags);
            
            System.Diagnostics.Debug.WriteLine("\n" + bibContext.ToString() + "\n");
            return View(await bibContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string selectedBooks)
        {
            System.Diagnostics.Debug.WriteLine(selectedBooks);

            var bibContext = _context.Books.Include(b => b.category).Include(b => b.authors).Include(b => b.tags);

            if (string.IsNullOrEmpty(selectedBooks))return View(await bibContext.ToListAsync());

            var selectedBooksList = selectedBooks.Split(',').Select(int.Parse).ToList();
            var result = bibContext.Where(p => selectedBooksList.Contains(p.bookId));
            return View(await result.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchBooks(List<int> selectedBooks)
        {
            var bibContext = _context.Books.Include(b => b.category).Include(b => b.authors);
            var result = bibContext.Where(p => selectedBooks.Contains(p.bookId));
            return View(await bibContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Basket()
        {
            ViewData["BookList"] = await _context.Book.ToListAsync();
            ViewData["limits"] = await _context.AdminSettings.ToListAsync(); 
            return View();
        }

        [Authorize]
        public IActionResult Queue()
        {
            var user = _context.Readers.FirstOrDefault(r => r.email == User.Identity.Name);
            ViewData["userId"] = user == null ? 0 : user.id;
            return View();
        }

        public JsonResult GetBooks()
        {
            return Json(_context.Books.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

//book, rental, rentalbook