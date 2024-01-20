using MongoDB.Driver;
using VR_Labs_for_Higher_Education.Models;

namespace VR_Labs_for_Higher_Education.Services
{
    public class LabService
    {
        private readonly IMongoCollection<Student> _studentCollection;

        public LabService(IMongoDatabase database)
        {
            _studentCollection = database.GetCollection<Student>("students");
        }

        // API logic to communicate with the Unity Lab Simulation
        public async Task UpdateCheckpointTimestamp(string studentName, string labId, int checkpointIndex, string timestamp)
        {
            var studentFilter = Builders<Student>.Filter.Eq(s => s.Name, studentName);
            var updateDefinition = Builders<Student>.Update.Set(
                $"LabProgress.$.Checkpoints.{checkpointIndex}.Timestamp", timestamp);

            var labFilter = Builders<Student>.Filter.ElemMatch(s => s.LabProgress, lp => lp.LabId == labId);
            var combinedFilter = Builders<Student>.Filter.And(studentFilter, labFilter);

            var updateResult = await _studentCollection.UpdateOneAsync(
                combinedFilter,
                updateDefinition);

            // Log the result of the update operation
            if (updateResult.MatchedCount == 0)
            {
                // The student document was not found
                throw new KeyNotFoundException("No matching student document found.");
            }
            else if (updateResult.ModifiedCount == 0)
            {
                // The checkpoint was not found, or the timestamp was already set to the new value
                throw new Exception("The checkpoint was not updated. It might not exist or the timestamp was already set to this value.");
            }
        }
    }
}
