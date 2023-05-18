using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AltAir.Models;
using Route = AltAir.Models.Route;

namespace AltAir.Data
{

    public class AltAirContext : IdentityDbContext<IdentityUser>
    {
        public AltAirContext (DbContextOptions<AltAirContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; } = default!;

        public DbSet<Flight> Flights { get; set; } = default!;

        public DbSet<FlightClass> FlightClasses { get; set; } = default!;

        public DbSet<Layover> Layovers { get; set; } = default!;

        public DbSet<Plane> Planes { get; set; } = default!;

        public DbSet<Route> Routes { get; set; } = default!;

        public DbSet<Ticket> Tickets { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }
}
