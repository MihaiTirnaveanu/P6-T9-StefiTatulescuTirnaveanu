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
    public class FlightClassesController : Controller
    {
        private readonly AltAirContext _context;

        public FlightClassesController(AltAirContext context)
        {
            _context = context;
        }

        // GET: FlightClasses
        public async Task<IActionResult> Index()
        {
              return _context.FlightClasses != null ? 
                          View(await _context.FlightClasses.ToListAsync()) :
                          Problem("Entity set 'AltAirContext.FlightClasses'  is null.");
        }

        // GET: FlightClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FlightClasses == null)
            {
                return NotFound();
            }

            var flightClass = await _context.FlightClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flightClass == null)
            {
                return NotFound();
            }

            return View(flightClass);
        }

        // GET: FlightClasses/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlightClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassType")] FlightClass flightClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flightClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flightClass);
        }

        // GET: FlightClasses/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FlightClasses == null)
            {
                return NotFound();
            }

            var flightClass = await _context.FlightClasses.FindAsync(id);
            if (flightClass == null)
            {
                return NotFound();
            }
            return View(flightClass);
        }

        // POST: FlightClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassType")] FlightClass flightClass)
        {
            if (id != flightClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flightClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightClassExists(flightClass.Id))
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
            return View(flightClass);
        }

        // GET: FlightClasses/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FlightClasses == null)
            {
                return NotFound();
            }

            var flightClass = await _context.FlightClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flightClass == null)
            {
                return NotFound();
            }

            return View(flightClass);
        }

        // POST: FlightClasses/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FlightClasses == null)
            {
                return Problem("Entity set 'AltAirContext.FlightClasses'  is null.");
            }
            var flightClass = await _context.FlightClasses.FindAsync(id);
            if (flightClass != null)
            {
                _context.FlightClasses.Remove(flightClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightClassExists(int id)
        {
          return (_context.FlightClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
