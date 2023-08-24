using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    [BsonIgnoreExtraElements]
    public class UserData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UserName")]
        public string? UserName { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("Age")]
        public int Age { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("PhoneNumber")]
        public string? Phonenumber { get; set; }

        [BsonElement("Password")]
        public string? Password { get; set; }

        [BsonElement("Role")]
        public string? Role { get; set; }

        [BsonElement("Status")]
        public string? Status { get; set; }
    }
}
