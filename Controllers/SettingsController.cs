using Chess_API.Database;
using Microsoft.AspNetCore.Mvc;

namespace Chess_API.Controllers;

public class SettingsController : Controller
{
    private readonly ChessDbContext _context;

    public SettingsController(ChessDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IActionResult Options()
    {
        return View();
    }
}