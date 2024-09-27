using Application.Domain.Model.Finance;

namespace Application.Domain.Model.User
{
    public class UserFinance
    {
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
