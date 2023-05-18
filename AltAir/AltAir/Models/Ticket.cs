using AltAir.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltAir.Models
{
    public class Ticket
    {
        public int Id { get; set; }      
        public int Seat { get; set; }
        public double Price { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public int FlightClassId { get; set; }
        public FlightClass? FlightClass { get; set; }
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }
    }
}

