using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using VR_Labs_for_Higher_Education.Services;
using System.IdentityModel.Tokens.Jwt;
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = false;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the MongoDB connection string from the configuration
var mongoDBConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");

// Register IMongoDatabase with the necessary connection string and database name
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = new MongoClient(mongoDBConnectionString);
    return client.GetDatabase("users"); // Replace "users" with the actual database name if different
});

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = ctx =>
        {
            var idToken = ctx.SecurityToken as JwtSecurityToken;
            if (idToken != null)
            {
                // Log the raw ID token
                Console.WriteLine($"ID Token: {idToken.RawData}");
                // WARNING: Only log tokens in a development environment. Never log tokens in production.
            }
            return Task.CompletedTask;
        },
        // ... existing event handlers ...
    };
});

// Require authentication by default
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
    options.AddPolicy("InstructorOnly", policy => policy.RequireClaim("Role", "Instructor"));
    options.AddPolicy("StudentOnly", policy => policy.RequireClaim("Role", "Student"));
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<InstructorService>();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

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

app.UseAuthentication(); // Add this before app.UseAuthorization
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "studentHome",
        pattern: "Student/StudentHomePage",
        defaults: new { controller = "Student", action = "StudentHomePage" });

    endpoints.MapControllerRoute(
        name: "instructorHome",
        pattern: "Instructor/InstructorHomePage",
        defaults: new { controller = "Student", action = "InstructorHomePage" });
});


app.Run();