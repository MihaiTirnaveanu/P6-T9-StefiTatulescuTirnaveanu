namespace AltAir.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<Flight>? Flights { get; set;}
        public ICollection<Layover>? Layovers { get; set;}
    }
}
