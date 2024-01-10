using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VR_Labs_for_Higher_Education.Models
{
    public class Lab
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("labSim")]
        public string LabPathway { get; set; }
        [BsonElement("tutorialVid")]
        public string TutorialPath { get; set; }
        [BsonElement("stages")]
        public List<LabStage> Stages { get; set; }
        [BsonElement("labGrade")]
        public double Grade { get; set; }


        public Lab()
        {
            Stages = new List<LabStage>();
        }
    }

    public class LabStage
    {
        public string StageName { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}