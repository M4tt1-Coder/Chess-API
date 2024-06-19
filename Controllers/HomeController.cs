using System.Diagnostics;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using Chess_API.Database;
using Chess_API.Models;
using Chess_API.utils;
using Microsoft.EntityFrameworkCore;

namespace Chess_API.Controllers;

public class HomeController : Controller
{
    private readonly ChessDbContext _context;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ChessDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}