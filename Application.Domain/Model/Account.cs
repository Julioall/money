namespace Application.Domain.Model
{
    public class Account
    {
        public string? Id { get; set; }
        public required string AccountName { get; set; }
        public required decimal Balance { get; set; }
        public required bool HideBalance { get; set; }
        public required string IconUrl { get; set; }
    }
}
