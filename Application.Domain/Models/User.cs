namespace Application.Domain.Models
{
    public class User
    {
        public string? Id { get; set; }
        public required UserAccount Account { get; set; }
        public required UserProfile Profile { get; set; }
        public required UserFinance Finance { get; set; }
    }
}
