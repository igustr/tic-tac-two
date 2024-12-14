using Domain;
using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public int SaveGame(string jsonStateString, string gameName);
    public List<string> GetSavedGamesNames();
    public GameState GetSavedGameByName(string name);
    public GameState LoadGame(int gameId);
}