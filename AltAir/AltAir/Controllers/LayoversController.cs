using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AltAir.Data;
using AltAir.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AltAir.Controllers
{
    public class LayoversController : Controller
    {
        private readonly AltAirContext _context;

        public LayoversController(AltAirContext context)
        {
            _context = context;
        }

        // GET: Layovers
        public async Task<IActionResult> Index()
        {
              return _context.Layovers != null ? 
                          View(await _context.Layovers.ToListAsync()) :
                          Problem("Entity set 'AltAirContext.Layovers'  is null.");
        }

        // GET: Layovers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Layovers == null)
            {
                return NotFound();
            }

            var layover = await _context.Layovers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (layover == null)
            {
                return NotFound();
            }

            return View(layover);
        }

        // GET: Layovers/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Layovers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Location")] Layover layover)
        {
            if (ModelState.IsValid)
            {
                _context.Add(layover);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(layover);
        }

        // GET: Layovers/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Layovers == null)
            {
                return NotFound();
            }

            var layover = await _context.Layovers.FindAsync(id);
            if (layover == null)
            {
                return NotFound();
            }
            return View(layover);
        }

        // POST: Layovers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location")] Layover layover)
        {
            if (id != layover.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(layover);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LayoverExists(layover.Id))
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
            return View(layover);
        }

        // GET: Layovers/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Layovers == null)
            {
                return NotFound();
            }

            var layover = await _context.Layovers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (layover == null)
            {
                return NotFound();
            }

            return View(layover);
        }

        // POST: Layovers/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Layovers == null)
            {
                return Problem("Entity set 'AltAirContext.Layovers'  is null.");
            }
            var layover = await _context.Layovers.FindAsync(id);
            if (layover != null)
            {
                _context.Layovers.Remove(layover);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LayoverExists(int id)
        {
          return (_context.Layovers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
