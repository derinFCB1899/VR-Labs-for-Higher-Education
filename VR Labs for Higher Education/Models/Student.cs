using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace VR_Labs_for_Higher_Education.Models
{
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

    public class LabProgress
    {
        [BsonElement("labId")]
        public string LabId { get; set; }

        [BsonElement("isComplete")]
        public bool IsComplete { get; set; }

        [BsonElement("grade")]
        public double? Grade { get; set; } // Nullable grade

        [BsonElement("completionTimestamp")]
        public DateTime? CompletionTimestamp { get; set; } // Nullable completion timestamp

        [BsonElement("checkpoints")]
        public List<LabCheckpoint> Checkpoints { get; set; }

        public LabProgress()
        {
            IsComplete = false;
            Checkpoints = new List<LabCheckpoint>();
        }
    }

    public class LabCheckpoint
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("timestamp")]
        public DateTime? Timestamp { get; set; }

        [BsonElement("index")]
        public int? Index { get; set; }

        public LabCheckpoint()
        {
            Timestamp = null;
            Index = null;
        }
    }

    public class Achievement
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("dateAchieved")]
        public DateTime DateAchieved { get; set; }
    }
}