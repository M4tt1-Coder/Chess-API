using Chess_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chess_API.Controllers;

public class SettingsController : Controller
{
    private readonly ChessDbContext _context;

    public SettingsController(ChessDbContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
}