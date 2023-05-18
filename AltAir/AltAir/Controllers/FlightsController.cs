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
    public class FlightsController : Controller
    {
        private readonly AltAirContext _context;

        public FlightsController(AltAirContext context)
        {
            _context = context;
        }

        // GET: Flights
        public async Task<IActionResult> Index(string childname)
        {
            var altAirContext = _context.Flights.Include(f => f.Plane).Include(f => f.Route);
            var allFlights = await altAirContext.ToListAsync();

            var searchItems = await _context.Flights.Include(f => f.Plane).Include(f => f.Route)
                                        .Where(f => f.DepartureAirport == childname || f.ArrivalAirport == childname)
                                        .ToListAsync();

            if (String.IsNullOrEmpty(childname))
            {
                return View(allFlights);
            }
            else
            {
                return View(searchItems);
            }

        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Plane)
                .Include(f => f.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["PlaneId"] = new SelectList(_context.Planes, "Id", "Id");
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Description");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightDate,DepartureAirport,DepartureTime,ArrivalAirport,ArrivalTime,AvailableSeats,FlightType,RouteId,PlaneId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaneId"] = new SelectList(_context.Planes, "Id", "Id", flight.PlaneId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", flight.RouteId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["PlaneId"] = new SelectList(_context.Planes, "Id", "Id", flight.PlaneId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", flight.RouteId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightDate,DepartureAirport,DepartureTime,ArrivalAirport,ArrivalTime,AvailableSeats,FlightType,RouteId,PlaneId")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
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
            ViewData["PlaneId"] = new SelectList(_context.Planes, "Id", "Id", flight.PlaneId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", flight.RouteId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Plane)
                .Include(f => f.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flights == null)
            {
                return Problem("Entity set 'AltAirContext.Flights'  is null.");
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
          return (_context.Flights?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
