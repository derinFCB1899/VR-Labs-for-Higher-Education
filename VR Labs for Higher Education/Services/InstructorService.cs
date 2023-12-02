using VR_Labs_for_Higher_Education.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VR_Labs_for_Higher_Education.Services
{
    public class InstructorService
    {
        private readonly IMongoCollection<Instructor> _instructors;
        private readonly ILogger<InstructorService> _logger;

        public InstructorService(IMongoDatabase database, ILogger<InstructorService> logger)
        {
            _instructors = database.GetCollection<Instructor>("instructors");
            _logger = logger;
        }

        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            return await _instructors.Find(_ => true).ToListAsync();
        }

        public async Task<Instructor> GetInstructorAsync(string id)
        {
            return await _instructors.Find(instructor => instructor.Id == id).FirstOrDefaultAsync();
        }

        // Other instructor-specific methods can be added here

        public async Task<Instructor> EnsureInstructorRecord(string email, string fullName)
        {
            _logger.LogInformation($"Starting to ensure instructor record for email: {email}");

            try
            {
                var instructor = await _instructors.Find(s => s.Email == email).FirstOrDefaultAsync();

                if (instructor == null)
                {
                    _logger.LogInformation($"No instructor record found for email: {email}, creating new record.");

                    instructor = new Instructor
                    {
                        Email = email,
                        Name = fullName
                    };

                    await _instructors.InsertOneAsync(instructor);
                    _logger.LogInformation($"New instructor record created for email: {email}");
                }
                else
                {
                    _logger.LogInformation($"Existing instructor record found for email: {email}");
                }

                return instructor;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ensuring instructor record for email: {email}, Exception: {ex}");
                throw;
            }
        }


    }
}