using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VR_Labs_for_Higher_Education.Models
{
    public class Instructor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        public Instructor()
        {
            // Initialize any collections or default values here if needed
        }
    }
}
