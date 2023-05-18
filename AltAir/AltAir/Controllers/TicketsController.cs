using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AltAir.Data;
using AltAir.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AltAir.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AltAirContext _context;

        public TicketsController(AltAirContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<Ticket> altAirContext;

            if (User.IsInRole("Administrator"))
            {
                altAirContext = _context.Tickets.Include(t => t.ApplicationUser).Include(t => t.Flight).Include(t => t.FlightClass);
            } else
            {
                altAirContext = _context.Tickets.Include(t => t.ApplicationUser).Include(t => t.Flight).Include(t => t.FlightClass).Where(t => t.ApplicationUserId == userId);
            }
                return View(await altAirContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.ApplicationUser)
                .Include(t => t.Flight)
                .Include(t => t.FlightClass)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "User,Administrator")]
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id");
            ViewData["FlightClassId"] = new SelectList(_context.FlightClasses, "Id", "Id");
            return View();
        }
        [Authorize(Roles = "User,Administrator")]
        // GET: Tickets/ReserveTicket/5
        public IActionResult ReserveTicket(int flightId)
        {
            var flight = _context.Flights.FirstOrDefault(f => f.Id == flightId);

            if (flight == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticket = new Ticket
            {
                // cred ca application user id nu e necesar aici, dar flight id
                // este necesar pentru a aparea in view, in metoda de post cand se trimite formularul, sunt decat informatiile din el
                // deci va  trebui iar sa instantiezi si aceste fielduri care nu sunt setate de catre user in frontend
                ApplicationUserId = userId,
                FlightId = flight.Id,
                Flight = flight,
                ApplicationUser = (_context.Users.FirstOrDefault(u => u.Id == userId) as ApplicationUser),
            };

            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", flight.Id);
            ViewData["FlightClassId"] = new SelectList(_context.FlightClasses, "Id", "Id");

            return View(ticket);
        }
        [Authorize(Roles = "User,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReserveTicket([Bind("Seat,Price,FlightClassId")] Ticket ticket, int flightId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var flight = _context.Flights.FirstOrDefault(f => f.Id == flightId);
            var flightClass = _context.FlightClasses.FirstOrDefault(fc => fc.Id == ticket.FlightClassId);

            if (flight == null)
            {
                return NotFound();
            }

            ticket.ApplicationUserId = userId;
            ticket.FlightId = flight.Id;
            ticket.Flight = flight;
            ticket.Price = 50;
            ticket.ApplicationUser = (_context.Users.FirstOrDefault(u => u.Id == userId) as ApplicationUser);

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();

                flight.AvailableSeats -= ticket.Seat;
                _context.Update(flight);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            ViewData["FlightClassId"] = new SelectList(_context.FlightClasses, "Id", "Id", ticket.FlightClassId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", flightId);
            return View(ticket);
           
        }

        [Authorize(Roles = "Administrator,User")] //
        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Seat,Price,ApplicationUserId,FlightClassId,FlightId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // set the current user's ID as the ApplicationUserId
                ticket.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (ticket.Seat > 0 && ticket.Price >= 0)
                {
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seat must be greater than 0 and Price must be greater than or equal to 0.");
                }
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", ticket.ApplicationUserId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", ticket.FlightId);
            ViewData["FlightClassId"] = new SelectList(_context.FlightClasses, "Id", "Id", ticket.FlightClassId);
            return View(ticket);
        }



        [Authorize(Roles = "Administrator,User")]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", ticket.ApplicationUserId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", ticket.FlightId);
            ViewData["FlightClassId"] = new SelectList(_context.FlightClasses, "Id", "Id", ticket.FlightClassId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Seat,Price,ApplicationUserId,FlightClassId,FlightId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", ticket.ApplicationUserId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", ticket.FlightId);
            ViewData["FlightClassId"] = new SelectList(_context.FlightClasses, "Id", "Id", ticket.FlightClassId);
            return View(ticket);
        }

        //aici am lasat si user pentru ca nu stiu inca daca userul isi sterge ticketele lui sau poate sterge orice
        [Authorize(Roles = "User,Administrator")]
        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.ApplicationUser)
                .Include(t => t.Flight)
                .Include(t => t.FlightClass)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'AltAirContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        //
        //
        //
        // GET: Tickets/CancelTicket/5
        [Authorize(Roles = "User,Administrator")]
        public async Task<IActionResult> CancelTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // check if the current user is the owner of the ticket or an admin
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ticket.ApplicationUserId != userId && !User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            return View(ticket);
        }

        // POST: Tickets/CancelTicket/5
        [Authorize(Roles = "User,Administrator")]
        [HttpPost, ActionName("CancelTicket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelTicketConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // check if the current user is the owner of the ticket or an admin
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ticket.ApplicationUserId != userId && !User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult HasCard()
        {
            // Get the user ID of the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if there is a card for the current user
            bool hasCard = _context.Cards.Any(c => c.ApplicationUserId == userId);

            return Json(hasCard);
        }


    }
}
