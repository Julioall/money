namespace Application.Domain.Model
{
    public class UserFinance
    {
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<string> TransactionsIds { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
