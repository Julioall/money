namespace Application.Domain.Models
{
    public class UserAccount
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public DateTime? DateJoined { get; set; } = DateTime.Now.Date;

        public DateTime? LastLogin { get; set; }
        public string? LastLoginIp { get; set; }
        public string? LastLoginOperatingSystem { get; set; }
        public string? LastLoginBrowser { get; set; }
        public string? LastLoginLocation { get; set; }
    }
}
