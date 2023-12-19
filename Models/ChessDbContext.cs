using Microsoft.EntityFrameworkCore;

namespace Chess_API.Models;

public class ChessDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ChessDatabase");
    }
}