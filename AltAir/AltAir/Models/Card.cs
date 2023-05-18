using AltAir.Models;

namespace AltAir.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public string CardType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Cvc { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
