using Application.Domain.Model.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Domain.Model.Finance
{
    public class TransactionCategory
    {
        [BsonId]
        public string? Id { get; set; }
        public required string Name { get; set; }  // Ex: "Alimentação", "Transporte"
        public required string IconUrl { get; set; }
        public string? Description { get; set; }
        public CategoryType? Type { get; set; }
    }
}
