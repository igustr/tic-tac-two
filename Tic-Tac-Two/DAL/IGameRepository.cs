using Domain;
using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public int SaveGame(string jsonStateString, string gameConfigName, string gameName);
    public List<string> GetSavedGamesNames();
    public GameState GetSavedGameByName(string name);
    public GameState LoadGame(int gameId);
}