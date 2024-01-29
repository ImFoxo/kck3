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

namespace Biblioteka.Controllers
{
    public class AdminSettingsController : Controller
    {
        private readonly BibContext _context;

        public AdminSettingsController(BibContext context)
        {
            _context = context;
        }

        // GET: AdminSettings
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
              return _context.AdminSettings != null ? 
                          View(await _context.AdminSettings.ToListAsync()) :
                          Problem("Entity set 'BibContext.AdminSettings'  is null.");
        }

        // GET: AdminSettings/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdminSettings == null)
            {
                return NotFound();
            }

            var adminSettings = await _context.AdminSettings.FindAsync(id);
            if (adminSettings == null)
            {
                return NotFound();
            }
            return View(adminSettings);
        }

        // POST: AdminSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("adminSettingsId,limitTaken,limitWaiting,limitTimeTaken,limitTimeWaiting")] AdminSettings adminSettings)
        {
            if (id != adminSettings.adminSettingsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminSettings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminSettingsExists(adminSettings.adminSettingsId))
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
            return View(adminSettings);
        }
        
        private bool AdminSettingsExists(int id)
        {
          return (_context.AdminSettings?.Any(e => e.adminSettingsId == id)).GetValueOrDefault();
        }
    }
}
