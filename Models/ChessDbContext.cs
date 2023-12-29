using Microsoft.EntityFrameworkCore;

namespace Chess_API.Models;

public class ChessDbContext : DbContext
{
    public DbSet<GameModel> Game { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ChessDatabase");
    }
}