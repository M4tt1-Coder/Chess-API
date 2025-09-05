using Microsoft.EntityFrameworkCore;
using Chess_API.Models;
using Chess_API.utils;

namespace Chess_API.Database;

//Theory source:
//https://learn.microsoft.com/en-us/ef/core/modeling/

/// <summary>
/// Represents the database instance to store data in an in-memory database.
/// </summary>
public class ChessDbContext : DbContext
{
    /// <summary>
    /// Table for all figures on the board.
    /// </summary>
    public required DbSet<FigureModel> Figures { get; set; }
    
    /// <summary>
    /// Table for the fields.
    /// </summary>
    public required DbSet<FieldModel> Fields { get; set; }
    
    /// <summary>
    /// Table for the single field rows.
    /// </summary>
    public required DbSet<FieldRowModel> FieldRows { get; set; }
    
    /// <summary>
    /// Table for the player objects.
    /// </summary>
    public required DbSet<PlayerModel> Players { get; set; }
    
    /// <summary>
    /// Table for the game object.
    /// </summary>
    public required DbSet<GameModel> Game { get; init; }

    /// <summary>
    /// History of all moves in the current round.
    /// </summary>
    public required DbSet<MoveModel> Moves { get; set; }
    
    /// <summary>
    /// Adds the database service to the dependency injection.
    /// </summary>
    /// <param name="optionsBuilder">Configuration endpoint for the database.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ChessDatabase");
    }

    /// <summary>
    /// The new game instance is passed as an argument for saving.
    ///
    /// Fails if the args are null.
    /// </summary>
    /// <param name="updatedGame">Is the new game object with updated properties.</param>
    public async void UpdateGameModel(GameModel updatedGame)
    {
        var game = await GetGameModel();

        game = updatedGame;

        await this.SaveChangesAsync();
    }
    
    /// <summary>
    /// Gets the first object in the game table.
    ///
    /// Fails when the table is empty.
    /// </summary>
    /// <returns>The game instance</returns>
    public async Task<GameModel> GetGameModel()
    {
        return await Game.FirstAsync();
    }

    /// <summary>
    /// At the start of the game, the default game object will be saved.
    ///
    /// Fails when the entity can't be resolved.
    /// </summary>
    public async void AddDefaultGameModel()
    {
        var defaultGame = GameHandler.Default();

        Game.Add(defaultGame);
        await this.SaveChangesAsync();
    }

    /// <summary>
    /// Clears the game table.
    /// </summary>
    public async void DeleteGameModel()
    {
        var game = await GetGameModel();
        this.Remove(game);
        await this.SaveChangesAsync();
    }
}