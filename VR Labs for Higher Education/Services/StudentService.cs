using VR_Labs_for_Higher_Education.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity; // Ensure this using directive is added

public class StudentService
{
    private readonly IMongoCollection<Student> _students;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IMongoDatabase database, ILogger<StudentService> logger)
    {
        _students = database.GetCollection<Student>("students");
        _logger = logger;
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await _students.Find(_ => true).ToListAsync();
    }

    public async Task<Student> GetStudentAsync(string id)
    {
        return await _students.Find(student => student.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Student> EnsureStudentRecord(string email, string fullName)
    {
        _logger.LogInformation($"Starting to ensure student record for email: {email}");

        try
        {
            var student = await _students.Find(s => s.Email == email).FirstOrDefaultAsync();

            if (student == null)
            {
                _logger.LogInformation($"No student record found for email: {email}, creating new record.");

                student = new Student
                {
                    Email = email,
                    Name = fullName,
                    LabProgress = new List<LabProgress>(),
                    Achievements = new List<Achievement>()
                };

                await _students.InsertOneAsync(student);
                _logger.LogInformation($"New student record created for email: {email}");
            }
            else
            {
                _logger.LogInformation($"Existing student record found for email: {email}");
            }

            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error ensuring student record for email: {email}, Exception: {ex}");
            throw;
        }
    }

    public async Task<Student> FindByEmailAsync(string email)
    {
        return await _students.Find(student => student.Email == email).FirstOrDefaultAsync();
    }

    public string HashPassword(string password)
    {
        var hasher = new PasswordHasher<Student>();
        return hasher.HashPassword(null, password);
    }

    public bool VerifyPassword(Student student, string providedPassword)
    {
        var hasher = new PasswordHasher<Student>();
        var result = hasher.VerifyHashedPassword(null, student.PasswordHash, providedPassword);
        _logger.LogWarning("Successfully matched passwords for student.");
        return result == PasswordVerificationResult.Success;
    }

}