// Used libraries
using VR_Labs_for_Higher_Education.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace VR_Labs_for_Higher_Education.Services
{
    public class StudentService
    {
        private readonly IMongoCollection<Student> _students;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IMongoDatabase database, ILogger<StudentService> logger)
        {
            _students = database.GetCollection<Student>("students");
            _logger = logger;
        }

        // Get list of all students
        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _students.Find(_ => true).ToListAsync();
        }

        // Find student by ID
        public async Task<Student> GetStudentAsync(string id)
        {
            return await _students.Find(student => student.Id == id).FirstOrDefaultAsync();
        }

        // Find student by email
        public async Task<Student> FindByEmailAsync(string email)
        {
            return await _students.Find(student => student.Email == email).FirstOrDefaultAsync();
        }

        // Hash student password
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<Student>();
            return hasher.HashPassword(null, password);
        }

        // Verify Student Login
        public bool VerifyPassword(Student student, string providedPassword)
        {
            var hasher = new PasswordHasher<Student>();
            var result = hasher.VerifyHashedPassword(null, student.PasswordHash, providedPassword);
            _logger.LogWarning("Successfully matched passwords for student.");
            return result == PasswordVerificationResult.Success;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            await _students.ReplaceOneAsync(s => s.Id == student.Id, student);
        }

    }
}