namespace Application.Domain.Dtos
{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? OperatingSystem { get; set; }
        public string? Browser { get; set; }
    }
}