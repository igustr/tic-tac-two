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

       //var config = _context.Configurations.First(c => c.Name == gameConfigName);

        var game = new Game()
        {
            GameName = gameName,
            GameState = jsonStateString
        };
        _context.Games.Add(game);
        _context.SaveChanges();
       // return game.Id;
    }

    public List<string> GetSavedGamesNames()
    {
        var savedGamesNames = _context.Configurations
            .OrderBy(c => c.Name)
            .Select(c => c.Name)
            .ToList();

        return savedGamesNames;
    }

    public GameState GetSavedGameByName(string name)
    {
        var data = _context.Configurations.First(c => c.Name == name);
        
        if (data == null)
        {
            // Log or throw a custom exception if needed
            throw new KeyNotFoundException($"Game with name '{name}' not found.");
        }
        
       // var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return null;
    }
}