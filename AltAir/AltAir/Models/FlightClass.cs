namespace AltAir.Models
{
    public class FlightClass
    {
        public int Id { get; set; } 
        public string ClassType { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
