namespace AltAir.Models
{
    public class Layover
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public ICollection<Route>? Routes { get; set; }

    }
}
