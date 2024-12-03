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
        var config = _context.Configurations.First(c => c.Name == gameConfigName);
        var game = new Game()
        {
            GameState = jsonStateString,
            NextMoveBy = 1, //TODO: CHANGE
            ConfigId = config.Id
        };
        _context.Games.Add(game);
        _context.SaveChanges();
       // return game.Id;
    }

    public List<string> GetSavedGamesNames()
    {
        throw new NotImplementedException();
    }

    public GameState GetSavedGame(string name)
    {
        throw new NotImplementedException();
    }
}