namespace Application.Domain.Models
{
    public class UserProfile
    {
        public required string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
