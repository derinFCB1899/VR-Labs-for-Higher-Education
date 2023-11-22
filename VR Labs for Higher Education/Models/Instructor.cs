namespace VR_Labs_for_Higher_Education.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System.Diagnostics.CodeAnalysis;

    public class Instructor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        // Add more properties as needed, such as phone number, address, etc.
    }

}
