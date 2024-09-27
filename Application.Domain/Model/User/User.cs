using Application.Domain.Model.Finance;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Domain.Model.User
{
    public class User
    {
        [BsonId]
        public string? Id { get; set; }
        public required UserAccount Account { get; set; }
        public required UserProfile Profile { get; set; }
    }
}
