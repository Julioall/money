using Application.Domain.Model.Enums;

namespace Application.Domain.Model
{
    public class Category
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public CategoryType? Type { get; set; }
        public required string Icon { get; set; }
        public required string Background { get; set; }
    }
}
