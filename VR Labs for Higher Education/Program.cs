using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using VR_Labs_for_Higher_Education.Services;
using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// MongoDB connection
var mongoDBConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = new MongoClient(mongoDBConnectionString);
    return client.GetDatabase("users");
});

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.SlidingExpiration = true;
    });


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

// Scoped services
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<InstructorService>();
builder.Services.AddScoped<LabService>();

var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Configure static file serving with custom MIME types
var options = new StaticFileOptions();
var contentTypeProvider = (FileExtensionContentTypeProvider)options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".wasm"] = "application/wasm"; // Adding .wasm MIME type
contentTypeProvider.Mappings[".data"] = "application/octet-stream"; // Ensure .data is still included
options.ContentTypeProvider = contentTypeProvider;
app.UseStaticFiles(options);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
});

app.Run();