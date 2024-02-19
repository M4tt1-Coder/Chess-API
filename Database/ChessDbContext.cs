using Microsoft.EntityFrameworkCore;
using Chess_API.Models;

namespace Chess_API.Database;

//TODO - Add usage functions for CRUD functionality

/// <summary>
/// Represents the database instance to store data in an in-memory database.
/// </summary>
public class ChessDbContext : DbContext
{
    /// <summary>
    /// The table for the fields on the board.
    /// </summary>
    //public DbSet<FieldModel> Fields { get; set; }
    
    /// <summary>
    /// The table for the eight rows of the playing field.
    /// </summary>
    //public DbSet<FieldRowModel> Rows { get; set; }
    
    /// <summary>
    /// The table for all figures in the game.
    /// </summary>
    //public DbSet<FigureModel> Figures { get; set; }
    
    /// <summary>
    /// The table for the players in the game.
    /// </summary>
    //public DbSet<PlayerModel> Players { get; set; }

    /// <summary>
    /// Table for the game object.
    /// </summary>
    public DbSet<GameModel> Game { get; set; }
    
    /// <summary>
    /// Adds the database service to the dependency injection.
    /// </summary>
    /// <param name="optionsBuilder">Configuration endpoint for the database.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ChessDatabase");
    }
}