using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Context;
using Biblioteka.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO.Compression;

namespace Biblioteka.Controllers
{
    public class BooksController : Controller
    {
        private readonly BibContext _context;

        public BooksController(BibContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            ViewData["limits"] = await _context.AdminSettings.ToListAsync();
            var bibContext = _context.Books.Include(b => b.category).Include(b => b.authors);
            return View(await bibContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.category)
                .Include(b => b.authors)
                .FirstOrDefaultAsync(m => m.bookId == id);
            if (book == null)
            {
                return NotFound();
            }
            var user = _context.Readers.FirstOrDefault(r => r.email == User.Identity.Name);
            ViewData["userId"] = user == null ? null : user.id;

            ViewData["fileAsString"] = book.fileAsString;
            ViewData["bookTitle"] = book.title;

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.AuthorList = new MultiSelectList(_context.Authors, "id", "name");
            ViewBag.TagList = new MultiSelectList(_context.Tag, "tagId", "tagName");
            ViewData["catId"] = new SelectList(_context.Set<Category>(), "catId", "catName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("bookId,title,ISBN,description,releaseDate,authors,tags,stockLevel,bookPhoto,catId,file")] Book book)
        {
            var authors = Request.Form["authors"];
            var tags = Request.Form["tags"];
            System.Diagnostics.Debug.WriteLine("\nIlość authors: " + (authors.Count) + "\n");
            System.Diagnostics.Debug.WriteLine("\nIlość tagów: " + (tags.Count) + "\n");

            if (book.file.Length > 0 && Path.GetExtension(book.file.FileName) == ".pdf")
            {
                using (var ms = new MemoryStream())
                {
                    book.file.CopyTo(ms);
                    book.fileAsString = Convert.ToBase64String(ms.ToArray());
                }
            }
            else
                ModelState.AddModelError("file not pdf", "Plik musi być w formacie PDF!");

            if (ModelState.IsValid)
            {
                foreach (var authorId in authors)
                {
                    var author = await _context.Authors.FindAsync(int.Parse(authorId));
                    string name = author.name;
                    if (author != null)
                    {
                        book.authors.Add(author);
                    }
                }

                foreach (var tagId in tags)
                {
                    var tag = await _context.Tag.FindAsync(int.Parse(tagId));
                    if (tag != null)
                    {
                        book.tags.Add(tag);
                    }
                }
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    if (modelStateVal.Errors.Count > 0)
                    {
                        foreach (var error in modelStateVal.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            ViewBag.AuthorList = new MultiSelectList(_context.Authors, "id", "name", book.authors);
            ViewBag.TagList = new MultiSelectList(_context.Tag, "tagId", "tagName", book.tags);
            ViewData["catId"] = new SelectList(_context.Set<Category>(), "catId", "catName", book.catId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.AuthorList = new MultiSelectList(_context.Authors, "id", "name", book.authors);
            ViewBag.TagList = new MultiSelectList(_context.Tag, "tagId", "tagName", book.tags);
            ViewData["catId"] = new SelectList(_context.Set<Category>(), "catId", "catName", book.catId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("bookId,title,ISBN,description,releaseDate,stockLevel,bookPhoto,catId")] Book book)
        {
            if (id != book.bookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.bookId))
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
            ViewData["catId"] = new SelectList(_context.Set<Category>(), "catId", "catName", book.catId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.category)
                .FirstOrDefaultAsync(m => m.bookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BibContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.bookId == id)).GetValueOrDefault();
        }
    }
}
