namespace AltAir.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public DateTime FlightDate { get; set; }
        public string DepartureAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public string FlightType { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int PlaneId { get; set; }
        public Plane? Plane { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
