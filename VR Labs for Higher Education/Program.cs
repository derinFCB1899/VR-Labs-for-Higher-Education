using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VR_Labs_for_Higher_Education.Data;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the MongoDB connection string from the configuration
var mongoDBConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
// Register MongoDbContext with the necessary connection string and database name
builder.Services.AddScoped(sp => new MongoDbContext(mongoDBConnectionString, "users"));
var mongoClient = new MongoClient(mongoDBConnectionString);
var mongoDatabase = mongoClient.GetDatabase("users");
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// If you're using ASP.NET Core Identity, set up a custom MongoDB store here
// Otherwise, remove Identity services if you are not using it

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// If you're using ASP.NET Core Identity, add app.UseAuthentication() and app.UseAuthorization()

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();