namespace AltAir.Models
{
    public class Plane
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Manufacture { get; set; }
        public string Model { get; set; }
        public int NoSeats { get; set; }
        public ICollection<Flight>? Flights { get; set; }
    }
}
