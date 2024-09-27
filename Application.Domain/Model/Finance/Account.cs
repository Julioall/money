namespace Application.Domain.Model.Finance
{
    public class Account
    {
        public string? Id { get; set; }
        public required string AccountName { get; set; }
        public required decimal Balance { get; set; }
    }
}
