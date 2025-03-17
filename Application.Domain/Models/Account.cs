namespace Application.Domain.Models
{
    public class Account
    {
        public string? Id { get; set; }
        public required string AccountName { get; set; }
        public decimal? Balance { get; set; } = 0;
        public bool? HideBalance { get; set; } = false;
        public required string IconUrl { get; set; }
    }
}
