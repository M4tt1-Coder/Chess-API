using Chess_API.Models;
using Chess_API.utils;
using Chess_API.Database;
using Microsoft.EntityFrameworkCore.Internal;

//Initialize the environment variables loading
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ChessDbContext>();

//TODO - Create Game option pages to create the game instance
//TODO - Configure correct routing + in right order redirect -> use middlewares (when a side can't be accessed yet, ...)
//TODO - Finish playing page
//TODO - Write a checker for figure movements + game state

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();