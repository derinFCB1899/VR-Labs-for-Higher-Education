using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text.Json;
using VR_Labs_for_Higher_Education.Models;

public class TestController : Controller
{
    private readonly IMongoDatabase _mongoDatabase;

    public TestController(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public IActionResult Index()
    {
        try
        {
            var collection = _mongoDatabase.GetCollection<Student>("students");
            var document = collection.Find(Builders<Student>.Filter.Empty).FirstOrDefault();

            if (document != null)
            {
                var json = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
                return Content(json, "application/json");
            }
            else
            {
                return Content("MongoDB connection is successful, but no documents found in 'students' collection.");
            }
        }
        catch (Exception ex)
        {
            return Content($"Error connecting to MongoDB: {ex.Message}");
        }
    }
}