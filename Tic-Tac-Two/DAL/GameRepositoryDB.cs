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
    
    public int SaveGame(string jsonStateString, int gameId, string gameName)
    {
        // Check if the game already exists
        var existingGame = _context.Games.FirstOrDefault(g => g.Id == gameId);
        if (existingGame != null)
        {
            // Update the existing game's state
            existingGame.GameState = jsonStateString;
            _context.Games.Update(existingGame);
            _context.SaveChanges();
            return gameId;
        }
        
        Random random = new Random();
        var password = random.Next(0, 999999);
        
        // Insert a new game
        var newGame = new Game
        {
            GameState = jsonStateString,
            GameName = gameName,
            Password = password.ToString()
        };
        _context.Games.Add(newGame);
        _context.SaveChanges();
        return newGame.Id;
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
    
    public int GetIdByName(string name)
    {
        var data = _context.Games.First(c => c.GameName == name);
        
        if (data == null)
        {
            // Log or throw a custom exception if needed
            throw new KeyNotFoundException($"Game with name '{name}' not found.");
        }
        
        return data.Id;
    }
    
    public GameState LoadGame(int gameId)
    {
        var data = _context.Games.First(g => g.Id == gameId);
        
        if (data == null)
        {
            // Log or throw a custom exception if needed
            throw new KeyNotFoundException($"Game with id '{gameId}' not found.");
        }
        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(data.GameState);
        return gameState;
    }
}