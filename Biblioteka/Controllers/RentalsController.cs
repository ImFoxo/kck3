using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Context;
using Biblioteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Biblioteka.Controllers
{
    public class RentalsController : Controller
    {
        private readonly BibContext _context;

        public RentalsController(BibContext context)
        {
            _context = context;
        }

        // GET: Rentals
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var bibContext = _context.Rental.Include(r => r.user);
                return View(await bibContext.ToListAsync());
            }
            else
            {
                var user = _context.Readers.FirstOrDefault(r => r.email == User.Identity.Name);
                var bibContext = _context.Rental.Where(r => r.userId == user.id).Include(r => r.RentalBook).ThenInclude(rb => rb.book);                
                return View(await bibContext.ToListAsync());
            }
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rental == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental
                .Include(r => r.user)
                .FirstOrDefaultAsync(m => m.rentalId == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        [Authorize]
        public IActionResult Place()
        {
            ViewData["BookList"] = _context.Book.Select(c => new SelectListItem { Value = c.bookId.ToString(), Text = c.title }).ToList();
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public async Task<IActionResult> Place([Bind("rentalId,userE_mail,rentalDate,rentalState,stateDate,PESEL")] Rental rental, string selectedBooks)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToPage("Home");
            }
            var user = _context.Readers.FirstOrDefault(r => r.email == User.Identity.Name);
            rental.userId = user.id;
            rental.rentalState = "Przyjęte";
            rental.rentalDate = DateTime.Now;
            rental.stateDate = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("\nRental : " + rental.userId + " " + rental.rentalState + " " + rental.rentalDate + " " + rental.stateDate + " " + rental.PESEL + "\n");

            System.Diagnostics.Debug.WriteLine(selectedBooks);

            _context.Add(rental);
            await _context.SaveChangesAsync();

            var selectedBooksList = selectedBooks.Split(';', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < selectedBooksList.Count(); i++)
            {
                System.Diagnostics.Debug.WriteLine(selectedBooksList[i]);
                var bookData = selectedBooksList[i].Split(',').ToList();
                for (int j = 0; j < bookData.Count; j++)
                {
                    System.Diagnostics.Debug.Write(bookData[j] + " ");
                }
                System.Diagnostics.Debug.Write("\n");
                _context.Add(new RentalBook(rental.rentalId, int.Parse(bookData[0]), int.Parse(bookData[1])));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rentals/Edit/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rental == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["userId"] = new SelectList(_context.Readers, "id", "email", rental.userId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("rentalId,userId,rentalDate,rentalState,stateDate,PESEL")] Rental rental)
        {
            if (id != rental.rentalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.rentalId))
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
            ViewData["userId"] = new SelectList(_context.Readers, "id", "email", rental.userId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rental == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental
                .Include(r => r.user)
                .FirstOrDefaultAsync(m => m.rentalId == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rental == null)
            {
                return Problem("Entity set 'BibContext.Rental'  is null.");
            }
            var rental = await _context.Rental.FindAsync(id);
            if (rental != null)
            {
                _context.Rental.Remove(rental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return (_context.Rental?.Any(e => e.rentalId == id)).GetValueOrDefault();
        }
    }
}
