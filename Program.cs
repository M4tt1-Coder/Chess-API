using Chess_API.utils;
using Chess_API.Database;

//Initialize the environment variables loading
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ChessDbContext>();
builder.Services.AddSingleton<IProtectionService, RoutesProtector>();

// TODO - add middleware checks when ever needed (*)  
// TODO - Check if a player has won with a checkmate
// TODO - Finish playing page
// TODO - Write a checker for figure movements + game state
// TODO - Add responsibility to all pages
// TODO - the special move castling for the king and the rook
// TODO - What to do when your king is in check
// TODO - Pawn reaches the end of the field -> promotion
// TODO - En passant

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
app.UseChessMiddleware();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();