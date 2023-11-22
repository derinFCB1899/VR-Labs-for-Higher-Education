using MongoDB.Bson;

namespace VR_Labs_for_Higher_Education.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Student
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

        [BsonElement("EnrollmentDate")]
        public DateTime EnrollmentDate { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }
    }


}
