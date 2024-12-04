using GameBrain;
using Domain;

namespace DAL;

public class GameRepositoryDB : IGameRepository
{
    private readonly DAL.AppDbContext _context;
    
    public GameRepositoryDB(AppDbContext context)
    {
        _context = context;
    }
    
    public void SaveGame(string jsonStateString, string gameConfigName, string gameName)
    {
        var game = new Game()
        {
            GameName = gameName,
            GameState = jsonStateString
        };
        _context.Games.Add(game);
        _context.SaveChanges();
    }

    public List<string> GetSavedGamesNames()
    {
        var savedGamesNames = _context.Games
            .OrderBy(c => c.GameName)
            .Select(c => c.GameName)
            .ToList();

        return savedGamesNames;
    }

    public GameState GetSavedGameByName(string name)
    {
        var data = _context.Games.First(c => c.GameName == name);
        
        if (data == null)
        {
            // Log or throw a custom exception if needed
            throw new KeyNotFoundException($"Game with name '{name}' not found.");
        }
        
        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(data.GameState);
        return gameState;
    }
}