namespace VR_Labs_for_Higher_Education.Data
{
    using MongoDB.Driver;
    using System;
    using VR_Labs_for_Higher_Education.Models;

    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Student> Students => _database.GetCollection<Student>("students");
        //public IMongoCollection<Instructor> Instructors => _database.GetCollection<Instructor>("instructors");
    }

}
