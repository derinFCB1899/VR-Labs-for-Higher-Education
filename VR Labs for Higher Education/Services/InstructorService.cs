using VR_Labs_for_Higher_Education.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
namespace VR_Labs_for_Higher_Education.Services
{
    public class InstructorService
    {
        private readonly IMongoCollection<Instructor> _instructors;
        private readonly IMongoCollection<Student> _studentCollection;
        private readonly ILogger<InstructorService> _logger;

        public InstructorService(IMongoDatabase database, ILogger<InstructorService> logger)
        {
            _instructors = database.GetCollection<Instructor>("instructors");
            _logger = logger;
            _studentCollection = database.GetCollection<Student>("students");
        }

        // List all instructors
        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            return await _instructors.Find(_ => true).ToListAsync();
        }

        // Find instructor by document ID
        public async Task<Instructor> GetInstructorAsync(string id)
        {
            return await _instructors.Find(instructor => instructor.Id == id).FirstOrDefaultAsync();
        }

        // Find instructor by email
        public async Task<Instructor> FindByEmailAsync(string email)
        {
            return await _instructors.Find(instructor => instructor.Email == email).FirstOrDefaultAsync();
        }

        // Password hashing script
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<Instructor>();
            return hasher.HashPassword(null, password);
        }


        // Verification of user password
        public bool VerifyPassword(Instructor instructor, string providedPassword)
        {
            var hasher = new PasswordHasher<Instructor>();
            var result = hasher.VerifyHashedPassword(null, instructor.PasswordHash, providedPassword);
            _logger.LogWarning("Successfully matched passwords for instructor.");
            return result == PasswordVerificationResult.Success;
        }

        // Scripts to get the list of students that completed the lab
        public async Task<List<BsonDocument>> GetStudentsCompletedLabAsync(string labId)
        {

            var filter = Builders<Student>.Filter.ElemMatch(
                s => s.LabProgress,
                lp => lp.LabId == labId && lp.IsComplete
            );

            var projection = Builders<Student>.Projection
                .Include(s => s.Name)
                .ElemMatch(s => s.LabProgress, lp => lp.LabId == labId);

            var studentsWithCompletedLabs = await _studentCollection
                .Find(filter)
                .Project<BsonDocument>(projection)
                .ToListAsync();

            return studentsWithCompletedLabs;
        }

        // Script for updating student grade
        public async Task<bool> UpdateStudentGradeAsync(string studentId, string labId, double grade)
        {

            var filter = Builders<Student>.Filter.And(
                Builders<Student>.Filter.Eq(s => s.Id, studentId),
                Builders<Student>.Filter.ElemMatch(s => s.LabProgress, lp => lp.LabId == labId)
            );

            var update = Builders<Student>.Update
                .Set("LabProgress.$.Grade", grade);

            var updateResult = await _studentCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }

    }
}