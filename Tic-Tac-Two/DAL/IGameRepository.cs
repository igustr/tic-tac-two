using Domain;
using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public int SaveGame(string jsonStateString, int gameId, string gameName);
    public List<string> GetSavedGamesNames();
    public GameState GetSavedGameByName(string name);
    public GameState LoadGame(int gameId);
}