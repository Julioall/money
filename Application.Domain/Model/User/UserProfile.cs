using Application.Domain.Model.Finance;

namespace Application.Domain.Model.User
{
    public class UserProfile
    {
        public required string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<CreditCard> CreditCards { get; set;} = new List<CreditCard>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new();
        public List<string> Tags { get; set; }
    }
}
