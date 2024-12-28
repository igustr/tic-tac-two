using Domain;
using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public int SaveGame(string jsonStateString, int gameId, string gameName);
    public List<string> GetSavedGamesNames();
    public GameState GetSavedGameByName(string name);
    public GameState LoadGame(int gameId);
    public int GetIdByName(string name);
    public void DeleteGame(int gameId);
    public int GetGameIdByPassword(string password);
}