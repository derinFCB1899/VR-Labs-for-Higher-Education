using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace VR_Labs_for_Higher_Education.Models
{
    public class LabProgress
    {
        public string LabId { get; set; }
        public DateTime LastWorkedOn { get; set; }
        public bool IsCompleted { get; set; }
        // Additional fields as needed
    }

    public class Achievement
    {
        public string Title { get; set; }
        public DateTime DateAchieved { get; set; }
    }

    public class Student
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


        [BsonElement("labProgress")]
        public List<LabProgress> LabProgress { get; set; }

        [BsonElement("achievements")]
        public List<Achievement> Achievements { get; set; } 

        public Student()
        {
            LabProgress = new List<LabProgress>();
            Achievements = new List<Achievement>();
        }
    }
}