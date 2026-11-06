namespace Application.Domain.Models
{
    public class Account
    {
        public string? Id { get; set; }
        public required string AccountName { get; set; }
        public decimal? Balance { get; set; } = 0;
        public bool? HideBalance { get; set; } = false;
        public required string IconUrl { get; set; }
        public string? BankName { get; set; }
        public string? BankIspb { get; set; }
        public string? AccountType { get; set; } = "checking";
    }
}
