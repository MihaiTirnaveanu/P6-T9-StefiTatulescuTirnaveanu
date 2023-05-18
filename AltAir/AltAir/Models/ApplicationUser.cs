using Microsoft.AspNetCore.Identity;

namespace AltAir.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }

        public ICollection<Card>? Cards { get; set; }
    }
}
